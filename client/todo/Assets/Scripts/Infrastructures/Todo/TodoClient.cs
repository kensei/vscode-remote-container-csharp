using System.Collections;
using System.Collections.Generic;
using Todo.Entity;

namespace Todo.Infrastructures.Todo
{
    public class TodoClient : ITodoClient
    {
        public List<TodoItem> GetTodoItems()
        {
            throw new System.NotImplementedException();
        }

        public TodoItem GetTodoItemById(long id)
        {
            throw new System.NotImplementedException();
        }

        public TodoItem AddTodoItem(TodoItem todoItem)
        {
            throw new System.NotImplementedException();
        }

        public TodoItem UpdateTodoItem(TodoItem todoItem)
        {
            throw new System.NotImplementedException();
        }

        public TodoItem DeleteTodoItem(long id)
        {
            throw new System.NotImplementedException();
        }
    }
}
