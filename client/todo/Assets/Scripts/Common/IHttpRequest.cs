using System;
using System.Collections;
using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;

namespace Todo.Common
{
    public interface IHttpRequest
    {
        public EnumHttpMethod Method { get; }

        public string URI { get; }

        public IRequestParam RequestParam { get; }

        public void OnError(int errorCode, string errorMassage);

        public void OnSuccess(IMessage httpResponse);
    }
}
