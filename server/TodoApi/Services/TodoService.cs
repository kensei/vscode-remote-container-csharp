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

        public async Task<TodoItem> AddTodoItem(TodoItem todoItem)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.TodoItems.Add(todoItem);
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                    return todoItem;
                }
                catch (DbUpdateConcurrencyException e)
                {
                    transaction.Rollback();
                    throw e;
                }
            }
        }

        public async Task<TodoItem> UpdateTodoItem(long id, TodoItem todoItem)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                _context.Entry(todoItem).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                    return todoItem;
                }
                catch (DbUpdateConcurrencyException e)
                {
                    transaction.Rollback();
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
        }

        public async Task<TodoItem> DeleteTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);
 
            // 指定したTodoレコードが存在しなかった場合、空のtodoItemを返して、コントローラーにその後の処理を託す。
            if (todoItem == null)
            {
                throw new ResourceNotFoundException($"todo is not found {id}");
            }
            using (var transaction = _context.Database.BeginTransaction()){
                try
                {
                  _context.TodoItems.Remove(todoItem);
                  await _context.SaveChangesAsync();
                  transaction.Commit();
                }
                catch (DbUpdateConcurrencyException e)
                {
                  transaction.Rollback();
                  throw e;
                }
            }
            return todoItem;
        }

        private bool TodoItemExists(long id)
        {
            return _context.TodoItems.Any(e => e.Id == id);
        }
    }
}