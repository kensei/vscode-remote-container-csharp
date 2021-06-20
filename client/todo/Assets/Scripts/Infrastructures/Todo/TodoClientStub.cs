using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Todo.Common;
using Todo.Entity;

namespace Todo.Infrastructures.Todo
{
    public class TodoClientStub : ITodoClient
    {
        private List<TodoItem> m_todoItems;

        public TodoClientStub()
        {
            m_todoItems = new List<TodoItem> {
                new TodoItem { Id = 1, Title = "hoge", IsComplete = true},
                new TodoItem { Id = 2, Title = "fuga", IsComplete = true},
                new TodoItem { Id = 3, Title = "piyo", IsComplete = true},
            };
        }

        public IEnumerator GetTodoItems(Action<List<TodoItem>> callback)
        {
            callback(m_todoItems);
            yield return null;
        }

        public IEnumerator GetTodoItemById(long id, Action<TodoItem> callback)
        {
            var todo = m_todoItems.Find(x => x.Id == id);
            if (todo == null)
            {
                throw new NotFoundException("not found id:" + id);
            }
            callback(todo);
            yield return null;
        }

        public IEnumerator AddTodoItem(TodoItem todoItem, Action<TodoItem> callback)
        {
            todoItem.Id = m_todoItems.OrderBy(x => x.Id).LastOrDefault().Id + 1;
            m_todoItems.Add(todoItem);
            callback(todoItem);
            yield return null;
        }

        public IEnumerator UpdateTodoItem(TodoItem todoItem, Action<TodoItem> callback)
        {
            var updateIndex = m_todoItems.FindIndex(x => x.Id == todoItem.Id);
            if (updateIndex < 0)
            {
                throw new NotFoundException("not found id:" + todoItem.Id);
            }
            m_todoItems[updateIndex] = todoItem;
            callback(todoItem);
            yield return null;
        }

        public IEnumerator DeleteTodoItem(long id, Action<TodoItem> callback)
        {
            var removeItemEnumrator = GetTodoItemById(id, (removeItem) => {
                callback(removeItem);
                m_todoItems.Remove(removeItem);
            });
            yield return null;
        }
    }
}
