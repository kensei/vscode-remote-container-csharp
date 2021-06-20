using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Google.Protobuf;
using Todo.Common;
using Todo.Entity;
using TodoApi.Pb.Messages.Todo;

public class TodosGetRequest : IHttpRequest
{
    private Dictionary<string, object> m_requestParam;
    private Action<List<TodoItem>> m_successHandler;
    private Action<int, string> m_errorHandler;

    public EnumHttpMethod Method
    {
        get { return EnumHttpMethod.GET; }
    }

    public string URI
    {
        get { return "TodoItems"; }
    }

    public Dictionary<string, object> RequestParam
    {
        get { return m_requestParam; }
    }

    public TodosGetRequest(Action<List<TodoItem>> successHandler, Action<int, string> errorHandler)
    {
        m_successHandler = successHandler;
        m_errorHandler = errorHandler;
    }

    public void OnError(int errorCode, string errorMassage)
    {
        m_errorHandler(errorCode, errorMassage);
    }

    public void OnSuccess(IMessage httpResponse)
    {
        var todoGetResponse = (TodosGetResponse)httpResponse;
        var todoItems = todoGetResponse.TodoItems.Select(x => new TodoItem { Id = x.Id, Title = x.Title, IsComplete = x.IsComplete }).ToList();
        m_successHandler(todoItems);
    }
}
