using System;
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
            //m_client = new TodoClientStub();
            m_client = new TodoClient();
        }

        public IEnumerator GetTodoItems(Action<List<TodoItem>> callback)
        {
            yield return m_client.GetTodoItems(callback);
        }

        public IEnumerator GetTodoItemById(int id, Action<TodoItem> callback)
        {
            yield return m_client.GetTodoItemById(id, callback);
        }

        public IEnumerator AddTodoItem(TodoItem todoItem, Action<TodoItem> callback)
        {
            yield return m_client.AddTodoItem(todoItem, callback);
        }

        public IEnumerator UpdateTodoItem(TodoItem todoItem, Action<TodoItem> callback)
        {
            yield return m_client.UpdateTodoItem(todoItem, callback);
        }

        public IEnumerator DeleteTodoItem(long id, Action<TodoItem> callback)
        {
            yield return m_client.DeleteTodoItem(id, callback);
        }
    }
}
