using System.Collections;
using System.Collections.Generic;
using Todo.Entity;

namespace Todo.Infrastructures.Todo
{
    public interface ITodoClient
    {
        public List<TodoItem> GetTodoItems();

        public TodoItem GetTodoItemById(long id);

        public TodoItem AddTodoItem(TodoItem todoItem);

        public TodoItem UpdateTodoItem(TodoItem todoItem);

        public TodoItem DeleteTodoItem(long id);
    }
}