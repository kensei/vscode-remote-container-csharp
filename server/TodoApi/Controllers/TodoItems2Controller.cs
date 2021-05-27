using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TodoApi.Data;
using TodoApi.Models;
using TodoApi.Services;
using TodoApi.Pb.Messages;
using TodoApi.Pb.Messages.Todo;
using System.IO;
using Google.Protobuf;

namespace server.Controllers
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/TodoItems")]
    [ApiController]
    public class TodoItems2Controller : ControllerBase
    {
        private readonly TodoContext _context;
        private readonly TodoService _service;
        private readonly MyOption _option;

        public TodoItems2Controller(TodoContext context, IOptions<MyOption> optionsAccessor)
        {
            _context = context;
            _service = new TodoService(context);
            _option = optionsAccessor.Value;
        }

        // GET: api/TodoItems
        [HttpGet]
        public async Task<IActionResult> GetTodoItems()
        {
            var todoItems = await _service.GetTodoItems();

            var commonResponse = new CommonDataResponse{ ErrorCode = (int)ResponseStatus.Success, ErrorMessage = "" };
            var resultTodoItems = todoItems.Select(x => new TodoApi.Pb.Messages.TodoItemData { Id = x.Id,  Title = x.Name, IsComplete = x.IsComplete });

            var responseObject = new TodosGetResponse { Common = commonResponse };
            responseObject.TodoItems.AddRange(resultTodoItems);

            var response = Serialize<TodosGetResponse>(responseObject);

            if (_option.IsLoggingJson)
            {
                var logObject = Deserialize<TodosGetResponse>(response);
                Console.WriteLine(logObject.ToString());
            }

            return File(response, "application/octet-stream");
        }

        static byte[] Serialize<T>(T obj) where T : IMessage<T> {
            using (var stream = new MemoryStream())
            {
                obj.WriteTo(stream);
                return stream.ToArray();
            }
        }

        static T Deserialize<T>(byte[] data) where T : IMessage<T>, new()
        {
            var parser = new MessageParser<T>(() => new T());
            return parser.ParseFrom(new MemoryStream(data));
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
    }
}
