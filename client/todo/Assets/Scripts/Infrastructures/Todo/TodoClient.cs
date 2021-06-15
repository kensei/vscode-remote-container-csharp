using System.Collections;
using System.Collections.Generic;
using Todo.Entity;

namespace Todo.Infrastructures.Todo
{
    public class TodoClient : ITodoClient
    {
        public IEnumerator<List<TodoItem>> GetTodoItems()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerator<TodoItem> GetTodoItemById(long id)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerator<TodoItem> AddTodoItem(TodoItem todoItem)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerator<TodoItem> UpdateTodoItem(TodoItem todoItem)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerator<TodoItem> DeleteTodoItem(long id)
        {
            throw new System.NotImplementedException();
        }
    }
}
