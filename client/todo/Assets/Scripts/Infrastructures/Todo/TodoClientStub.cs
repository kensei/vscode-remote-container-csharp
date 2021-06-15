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

        public IEnumerator<List<TodoItem>> GetTodoItems()
        {
            yield return m_todoItems;
        }

        public IEnumerator<TodoItem> GetTodoItemById(long id)
        {
            var todo = m_todoItems.Find(x => x.Id == id);
            if (todo == null)
            {
                throw new NotFoundException("not found id:" + id);
            }
            yield return todo;
        }

        public IEnumerator<TodoItem> AddTodoItem(TodoItem todoItem)
        {
            todoItem.Id = m_todoItems.OrderBy(x => x.Id).LastOrDefault().Id + 1;
            m_todoItems.Add(todoItem);
            yield return todoItem;
        }

        public IEnumerator<TodoItem> UpdateTodoItem(TodoItem todoItem)
        {
            var updateIndex = m_todoItems.FindIndex(x => x.Id == todoItem.Id);
            if (updateIndex < 0)
            {
                throw new NotFoundException("not found id:" + todoItem.Id);
            }
            m_todoItems[updateIndex] = todoItem;
            yield return todoItem;
        }

        public IEnumerator<TodoItem> DeleteTodoItem(long id)
        {
            var removeItemEnumrator = GetTodoItemById(id);
            while(removeItemEnumrator.MoveNext())
            {
                var removeItem = removeItemEnumrator.Current;
                m_todoItems.Remove(removeItem);
                yield return removeItem;
            }
        }
    }
}
