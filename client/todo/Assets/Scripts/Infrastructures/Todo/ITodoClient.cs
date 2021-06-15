using System.Collections;
using System.Collections.Generic;
using Todo.Entity;

namespace Todo.Infrastructures.Todo
{
    public interface ITodoClient
    {
        public IEnumerator<List<TodoItem>> GetTodoItems();

        public IEnumerator<TodoItem> GetTodoItemById(long id);

        public IEnumerator<TodoItem> AddTodoItem(TodoItem todoItem);

        public IEnumerator<TodoItem> UpdateTodoItem(TodoItem todoItem);

        public IEnumerator<TodoItem> DeleteTodoItem(long id);
    }
}