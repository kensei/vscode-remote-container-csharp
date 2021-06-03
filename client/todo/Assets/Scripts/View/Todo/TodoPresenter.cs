using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Todo.Entity;
using Todo.Repositories;
using Todo.Screen.ViewModels;
using Todo.Screen.Views;

namespace Todo.Screen
{
    public class TodoPresenter
    {
        private TodoView m_view;
        private TodoRepository m_repository;

        public TodoPresenter(TodoView view, TodoRepository repository)
        {
            m_view = view;
            m_repository = repository;
        }

        public void Start()
        {
            var todoList = m_repository.GetTodoItems().Select(x => ConvertModelToViewModel(x)).ToList();
            m_view.Show(todoList);
        }

        private TodoItemViewModel ConvertModelToViewModel(TodoItem model)
        {
            return new TodoItemViewModel { Title = model.Title, IsComplete = model.IsComplete };
        }
    }
}
