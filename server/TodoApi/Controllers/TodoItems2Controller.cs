using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

        // GET: api/TodoItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CommonResponse<TodoItem>>> GetTodoItem(long id)
        {
            TodoItem result = null;

            try {
                result = await _service.GetTodoItem(id);
                return GenerateSucessResponse<TodoItem>(result);
            } catch (ResourceNotFoundException e) {
                return GenerateNotFoundResponse<TodoItem>(e);
            } catch (Exception e) {
                return GenerateFailResponse<TodoItem>(e);
            }
        }

        // PUT: api/TodoItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<CommonResponse<TodoItem>>> PutTodoItem(long id, TodoItem todoItem)
        {
            if (id != todoItem.Id)
            {
                return BadRequest();
            }
            try {
                var result = await _service.UpdateTodoItem(id, todoItem);
                return GenerateSucessResponse<TodoItem>(result);
            } catch (ResourceNotFoundException e) {
                return GenerateNotFoundResponse<TodoItem>(e);
            } catch (Exception e) {
                return GenerateFailResponse<TodoItem>(e);
            }
        }

        // POST: api/TodoItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CommonResponse<TodoItem>>> PostTodoItem(TodoItem todoItem)
        {
            try {
                var result = await _service.AddTodoItem(todoItem);
                return GenerateSucessResponse<TodoItem>(result);
            } catch (ResourceNotFoundException e) {
                return GenerateNotFoundResponse<TodoItem>(e);
            } catch (Exception e) {
                return GenerateFailResponse<TodoItem>(e);
            }
        }

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CommonResponse<TodoItem>>> DeleteTodoItem(long id)
        {
            try {
                var result = await _service.DeleteTodoItem(id);
                return GenerateSucessResponse<TodoItem>(result);
            } catch (ResourceNotFoundException e) {
                return GenerateNotFoundResponse<TodoItem>(e);
            } catch (Exception e) {
                return GenerateFailResponse<TodoItem>(e);
            }
        }

        private CommonResponse<T> GenerateSucessResponse<T>(T data)
        {
            var result = new CommonResponse<T>();
            result.ErrorCode = (int)ResponseStatus.Success;
            result.Data = data;
            return result;
        }
        
        private CommonResponse<T> GenerateFailResponse<T>(Exception e)
        {
            var result = new CommonResponse<T>();
            result.ErrorCode = (int)ResponseStatus.Fail;
            return result;
        }

        private CommonResponse<T> GenerateNotFoundResponse<T>(Exception e)
        {
            var result = new CommonResponse<T>();
            result.ErrorCode = (int)ResponseStatus.NotFound;
            return result;
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
    }
}
