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
            var todoItemsIterator = m_repository.GetTodoItems();
            yield return todoItemsIterator;
            var todoList = todoItemsIterator.Current.Select(x => ConvertModelToViewModel(x)).ToList();
            yield return m_view.Show(todoList, AddTodoItem(), UpdateTodoItem, DeleteTodoItem);
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
            m_repository.DeleteTodoItem(deleteTodoItem.Id);
            yield return m_view.DeleteElement(deleteTodoItemViewModel);
        }

        private IEnumerator DialogAddResultHandler(TodoDialogViewModel viewModel)
        {
            var addTodoItem = ConvertDialogViewModelToModel(viewModel);
            var addTodoItemIterator = m_repository.AddTodoItem(addTodoItem);
            yield return addTodoItemIterator;
            var addTodoModel = addTodoItemIterator.Current;
            var addTodoViewModel = ConvertModelToViewModel(addTodoModel);
            yield return m_view.AddElement(addTodoViewModel);
        }

        private IEnumerator DialogUpdateResultHandler(TodoDialogViewModel viewModel)
        {
            var updateTodoItem = ConvertDialogViewModelToModel(viewModel);
            var updateTodoItemItarator = m_repository.UpdateTodoItem(updateTodoItem);
            yield return updateTodoItemItarator;
            var updateTodoModel = updateTodoItemItarator.Current;
            var updateTodoViewModel = ConvertModelToViewModel(updateTodoModel);
            yield return m_view.UpdateElement(updateTodoViewModel);
        }

        private void DialogCancelHandler()
        {
            UnityEngine.Debug.Log("DialogCancelHandler");
        }
    }
}
