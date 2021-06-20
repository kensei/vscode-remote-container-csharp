using System;
using System.Collections;
using System.Collections.Generic;
using Todo.Entity;

namespace Todo.Infrastructures.Todo
{
    public interface ITodoClient
    {
        public IEnumerator GetTodoItems(Action<List<TodoItem>> callback);

        public IEnumerator GetTodoItemById(long id, Action<TodoItem> callback);

        public IEnumerator AddTodoItem(TodoItem todoItem, Action<TodoItem> callback);

        public IEnumerator UpdateTodoItem(TodoItem todoItem, Action<TodoItem> callback);

        public IEnumerator DeleteTodoItem(long id, Action<TodoItem> callback);
    }
}