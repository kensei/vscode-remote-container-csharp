using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Services
{
    public class TodoService
    {
        private readonly TodoContext _context;
        public TodoService(TodoContext context)
        {
            _context = context;
        }

        public async Task<List<TodoItem>> GetTodoItems()
        {
            return await _context.TodoItems.ToListAsync();
        }

        public async Task<TodoItem> GetTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                throw new ResourceNotFoundException(string.Format("todo not found {0}", id));
            }

            return todoItem;
        }

        public async Task<TodoItem> UpdateTodoItem(long id, TodoItem todoItem)
        {
            _context.Entry(todoItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return await GetTodoItem(id);
            }
            catch (DbUpdateConcurrencyException e)
            {
                if (!TodoItemExists(id))
                {
                    throw new ResourceNotFoundException(string.Format("todo not found {0}", id));
                }
                else
                {
                    throw e;
                }
            }
        }

        private bool TodoItemExists(long id)
        {
            return _context.TodoItems.Any(e => e.Id == id);
        }
    }
}