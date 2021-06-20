using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Todo.Entity;
using Todo.Repositories;
using Todo.Views.Todo.Controllers;
using Todo.Views.Todo.ViewModels;
using Todo.Views.Todo.Views;

namespace Todo.Views.Todo
{
    public class TodoPresenter
    {
        private TodoView m_view;
        private TodoDialogController m_todoDialog;
        private TodoRepository m_repository;

        public TodoPresenter(TodoView view, TodoDialogController todoDialog, TodoRepository repository)
        {
            m_view = view;
            m_todoDialog = todoDialog;
            m_repository = repository;
        }

        public IEnumerator Start()
        {
            yield return m_repository.GetTodoItems((todoList) => {
                var todoViewList = todoList.Select(x => ConvertModelToViewModel(x)).ToList();
                m_view.Show(todoViewList, AddTodoItem(), UpdateTodoItem, DeleteTodoItem);
            });
        }

        private TodoItemViewModel ConvertModelToViewModel(TodoItem model)
        {
            return new TodoItemViewModel { Id = model.Id, Title = model.Title, IsComplete = model.IsComplete };
        }

        private TodoItem ConvertViewModelToModel(TodoItemViewModel viewModel)
        {
            return new TodoItem { Id = viewModel.Id, Title = viewModel.Title, IsComplete = viewModel.IsComplete };
        }

        private TodoItem ConvertDialogViewModelToModel(TodoDialogViewModel viewModel)
        {
            return new TodoItem { Id = viewModel.Id, Title = viewModel.Title, IsComplete = viewModel.IsComplete };
        }

        private TodoDialogViewModel ConvertViewModelToDialogViewModel(TodoItemViewModel viewModel, TodoDialogViewModel.DialogViewMode dialogViewMode)
        {
            return new TodoDialogViewModel { TodoDialogViewMode = dialogViewMode, Id = viewModel.Id, Title = viewModel.Title, IsComplete = viewModel.IsComplete };
        }

        private IEnumerator AddTodoItem()
        {
            UnityEngine.Debug.Log("AddTodoItem");
            var newItem = new TodoDialogViewModel { TodoDialogViewMode = TodoDialogViewModel.DialogViewMode.Add };
            yield return m_todoDialog.Show(newItem, DialogAddResultHandler, DialogCancelHandler);
        }

        private IEnumerator UpdateTodoItem(TodoItemViewModel updateTodoItemViewModel)
        {
            UnityEngine.Debug.Log("UpdateTodoItem:" + updateTodoItemViewModel.Id);
            var updateItem = ConvertViewModelToDialogViewModel(updateTodoItemViewModel, TodoDialogViewModel.DialogViewMode.Edit); ;
            yield return m_todoDialog.Show(updateItem, DialogUpdateResultHandler, DialogCancelHandler);
        }

        private IEnumerator DeleteTodoItem(TodoItemViewModel deleteTodoItemViewModel)
        {
            UnityEngine.Debug.Log("DeleteTodoItem:" + deleteTodoItemViewModel.Id);
            var deleteTodoItem = ConvertViewModelToModel(deleteTodoItemViewModel);
            yield return m_repository.DeleteTodoItem(deleteTodoItem.Id, (todoItem) =>
            {
                m_view.DeleteElement(deleteTodoItemViewModel);
            });
        }

        private IEnumerator DialogAddResultHandler(TodoDialogViewModel viewModel)
        {
            var addTodoItem = ConvertDialogViewModelToModel(viewModel);
            yield return m_repository.AddTodoItem(addTodoItem, (todoItem) =>
            {
                var addTodoViewModel = ConvertModelToViewModel(todoItem);
                m_view.AddElement(addTodoViewModel);
            });            
        }

        private IEnumerator DialogUpdateResultHandler(TodoDialogViewModel viewModel)
        {
            var updateTodoItem = ConvertDialogViewModelToModel(viewModel);
            yield return m_repository.UpdateTodoItem(updateTodoItem, (todoItem) =>
            {
                var updateTodoViewModel = ConvertModelToViewModel(todoItem);
                m_view.UpdateElement(updateTodoViewModel);
            });
        }

        private void DialogCancelHandler()
        {
            UnityEngine.Debug.Log("DialogCancelHandler");
        }
    }
}
