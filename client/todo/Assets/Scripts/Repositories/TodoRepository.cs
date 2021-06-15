using System.Collections;
using System.Collections.Generic;
using Todo.Infrastructures.Todo;
using Todo.Entity;

namespace Todo.Repositories
{
    public class TodoRepository
    {
        ITodoClient m_client;

        public TodoRepository()
        {
            m_client = new TodoClientStub();
        }

        public IEnumerator<List<TodoItem>> GetTodoItems()
        {
            return m_client.GetTodoItems();
        }

        public IEnumerator<TodoItem> GetTodoItemById(int id)
        {
            return m_client.GetTodoItemById(id);
        }

        public IEnumerator<TodoItem> AddTodoItem(TodoItem todoItem)
        {
            return m_client.AddTodoItem(todoItem);
        }

        public IEnumerator<TodoItem> UpdateTodoItem(TodoItem todoItem)
        {
            return m_client.UpdateTodoItem(todoItem);
        }

        public IEnumerator<TodoItem> DeleteTodoItem(long id)
        {
            return m_client.DeleteTodoItem(id);
        }
    }
}
