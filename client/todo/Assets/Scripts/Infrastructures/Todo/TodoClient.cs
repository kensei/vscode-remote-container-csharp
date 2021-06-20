using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Todo.Entity;
using Todo.Utils;
using TodoApi.Pb.Messages.Todo;

namespace Todo.Infrastructures.Todo
{
    public class TodoClient : ITodoClient
    {
        public IEnumerator GetTodoItems(Action<List<TodoItem>> callback)
        {
            var request = new TodosGetRequest(callback, OnErrorHandler);

            yield return HttpUtils.Instance.Execute<TodosGetResponse>(request);
        }

        public IEnumerator GetTodoItemById(long id, Action<TodoItem> callback)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerator AddTodoItem(TodoItem todoItem, Action<TodoItem> callback)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerator UpdateTodoItem(TodoItem todoItem, Action<TodoItem> callback)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerator DeleteTodoItem(long id, Action<TodoItem> callback)
        {
            throw new System.NotImplementedException();
        }

        void OnErrorHandler(int errorCode, string errorMassage)
        {
            UnityEngine.Debug.LogError($"onerror {errorCode} {errorMassage}");
        }
    }
}
