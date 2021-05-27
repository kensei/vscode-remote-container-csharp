using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TodoApi.Data;
using TodoApi.Models;
using TodoApi.Services;

namespace server.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/TodoItems")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly TodoContext _context;
        private readonly TodoService _service;
        private readonly MyOption _option;

        public TodoItemsController(TodoContext context, IOptions<MyOption> optionsAccessor)
        {
            _context = context;
            _service = new TodoService(context);
            _option = optionsAccessor.Value;
        }

        // GET: api/TodoItems
        [HttpGet]
        public async Task<ActionResult<CommonResponse<List<TodoItem>>>> GetTodoItems()
        {
            var todoItems = await _service.GetTodoItems();
            return await GenerateSucessResponse<List<TodoItem>>(todoItems);
        }

        // GET: api/TodoItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(long id)
        {
            TodoItem result = null;

            try {
                result = await _service.GetTodoItem(id);
            } catch (ResourceNotFoundException) {
                return NotFound();
            }

            return result;
        }

        // PUT: api/TodoItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<TodoItem>> PutTodoItem(long id, TodoItem todoItem)
        {
            if (id != todoItem.Id)
            {
                return BadRequest();
            }

            TodoItem result = null;
            try {
                result = await _service.UpdateTodoItem(id, todoItem);
            } catch (ResourceNotFoundException) {
                return NotFound();
            } catch (Exception)
            {
                return BadRequest();
            }

            return NoContent();
        }

        // POST: api/TodoItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem todoItem)
        {
            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItem);
        }

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TodoItemExists(long id)
        {
            return _context.TodoItems.Any(e => e.Id == id);
        }

        private async Task<CommonResponse<T>> GenerateSucessResponse<T>(T data)
        {
            var result = new CommonResponse<T>();
            result.ErrorCode = (int)ResponseStatus.Success;
            result.Data = data;
            return result;
        }
    }
}
