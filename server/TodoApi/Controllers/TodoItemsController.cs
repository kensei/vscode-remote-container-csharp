using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
        private readonly TodoService _service;
        private readonly MyOption _option;

        public TodoItemsController(TodoService service, IOptions<MyOption> optionsAccessor)
        {
            _service = service;
            _option = optionsAccessor.Value;
        }

        // GET: api/TodoItems
        [HttpGet]
        public async Task<ActionResult<CommonResponse<List<TodoItem>>>> GetTodoItems()
        {
            var todoItems = await _service.GetTodoItems();
            return GenerateSucessResponse<List<TodoItem>>(todoItems);
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
    }
}
