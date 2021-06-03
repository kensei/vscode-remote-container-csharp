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

        public List<TodoItem> GetTodoItems()
        {
            return m_todoItems;
        }

        public TodoItem GetTodoItemById(long id)
        {
            var todo = m_todoItems.Find(x => x.Id == id);
            if (todo == null)
            {
                throw new NotFoundException("not found id:" + id);
            }
            return todo;
        }

        public TodoItem AddTodoItem(TodoItem todoItem)
        {
            todoItem.Id = m_todoItems.OrderBy(x => x.Id).LastOrDefault().Id + 1;
            m_todoItems.Add(todoItem);
            return todoItem;
        }

        public TodoItem UpdateTodoItem(TodoItem todoItem)
        {
            var updateIndex = m_todoItems.FindIndex(x => x.Id == todoItem.Id);
            if (updateIndex < 0)
            {
                throw new NotFoundException("not found id:" + todoItem.Id);
            }
            m_todoItems[updateIndex] = todoItem;
            return todoItem;
        }

        public TodoItem DeleteTodoItem(long id)
        {
            var removeItem = GetTodoItemById(id);
            m_todoItems.Remove(removeItem);
            return removeItem;
        }
    }
}
