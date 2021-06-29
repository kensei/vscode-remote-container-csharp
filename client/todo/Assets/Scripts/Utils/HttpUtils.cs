using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Google.Protobuf;
using Todo.Common;
using UnityEngine;
using UnityEngine.Networking;

namespace Todo.Utils
{
    public class HttpUtils : MonoBehaviour
    {
        private readonly string BASE_URI = "http://localhost:5001/api/v2.0/";
        private readonly int TIME_OUT = 15;
        private readonly int HTTP_ERROR_NO = 1000;
        private readonly int PB_PARSE_ERROR_NO = 2000;

        private static HttpUtils instance;
        public static HttpUtils Instance
        {
            get
            {
                if (instance == null)
                {
                    Debug.LogError("instance is nothing.");
                }

                return instance;
            }
        }

        private void Awake()
        {
            instance = this;
        }

        public IEnumerator Execute<PBResponse>(IHttpRequest request) where PBResponse : IMessage<PBResponse>, new()
        {
            switch (request.Method)
            {
                case EnumHttpMethod.GET:
                    using (UnityWebRequest webRequest = UnityWebRequest.Get(BASE_URI + request.URI))
                    {
                        yield return ExecuteHttpRequest<PBResponse>(request, webRequest);
                    }
                    break;
                case EnumHttpMethod.POST:
                    using (UnityWebRequest webRequest = UnityWebRequest.Post(BASE_URI + request.URI, ""))
                    {
                        SetupHttpRequest(request, webRequest);
                        webRequest.uploadHandler = new UploadHandlerRaw(ToByteArray(request.RequestParam.ToJson()));
                        webRequest.downloadHandler = new DownloadHandlerBuffer();
                        yield return ExecuteHttpRequest<PBResponse>(request, webRequest);
                    }
                    break;
                case EnumHttpMethod.PUT:
                    using (UnityWebRequest webRequest = UnityWebRequest.Post(BASE_URI + request.URI, request.RequestParam.ToJson()))
                    {
                        SetupHttpRequest(request, webRequest);
                        yield return ExecuteHttpRequest<PBResponse>(request, webRequest);
                    }
                    break;
                case EnumHttpMethod.DELETE:
                    using (UnityWebRequest webRequest = UnityWebRequest.Delete(BASE_URI + request.URI))
                    {
                        SetupHttpRequest(request, webRequest);
                        webRequest.downloadHandler = new DownloadHandlerBuffer();
                        yield return ExecuteHttpRequest<PBResponse>(request, webRequest);
                    }
                    break;
                default:
                    break;
            }
        }

        private IEnumerator ExecuteHttpRequest<PBResponse>(IHttpRequest request, UnityWebRequest webRequest) where PBResponse : IMessage<PBResponse>, new()
        {
            SetupHttpRequest(request, webRequest);

            yield return webRequest.SendWebRequest();

            try
            {
                HandleHttpRequst(webRequest.result);
            }
            catch (Exception e)
            {
                request.OnError(HTTP_ERROR_NO, e.Message);
            }

            try
            {
                var httpResponse = GenerateResponse<PBResponse>(webRequest);
                var commonResponse = GetCommonResponse(httpResponse);
                if (commonResponse.ErrorCode > 0)
                {
                    request.OnError(commonResponse.ErrorCode, commonResponse.ErrorMessage);
                }
                else
                {
                    request.OnSuccess(httpResponse);
                }
            }
            catch (ProtocolBufferParseException e)
            {
                request.OnError(PB_PARSE_ERROR_NO, e.Message);
            }
        }

        private TodoApi.Pb.Messages.CommonDataResponse GetCommonResponse<T>(T response) where T : IMessage<T>
        {
            var type = response.GetType();

            var res = (TodoApi.Pb.Messages.CommonDataResponse)type.GetProperty("Common").GetValue(response);

            return res;
        }

        private void SetupHttpRequest(IHttpRequest request, UnityWebRequest webRequest)
        {
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.timeout = TIME_OUT;
        }

        private byte[] ToByteArray(string json)
        {
            return Encoding.UTF8.GetBytes(json);
        }

        private void HandleHttpRequst(UnityWebRequest.Result result)
        {
            switch (result)
            {
                case UnityWebRequest.Result.InProgress:
                    Debug.Log("requesting now..");
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log("request success");
                    break;
                case UnityWebRequest.Result.ConnectionError:
                    throw new HttpException("ConnectionError");
                case UnityWebRequest.Result.ProtocolError:
                    throw new HttpException("ProtocolError");
                case UnityWebRequest.Result.DataProcessingError:
                    throw new HttpException("DataProcessingError");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private PBResponse GenerateResponse<PBResponse>(UnityWebRequest request) where PBResponse : IMessage<PBResponse>, new()
        {
            var byteData = (request.downloadHandler != null) ? request.downloadHandler.data : new byte[0];
#if DEBUG
            Debug.Log("length:" + byteData.Length);
            var sb = new StringBuilder("new byte[] { ");
            foreach (var b in byteData)
            {
                sb.Append(b + ", ");
            }
            sb.Append("}");
            Debug.Log("data:" + sb.ToString());
#endif
            var response = Deserialize<PBResponse>(byteData);

            return response;
        }

        private T Deserialize<T>(byte[] data) where T : IMessage<T>, new()
        {
            var parser = new MessageParser<T>(() => new T());
            try
            {
                return parser.ParseFrom(new MemoryStream(data));
            }
            catch (Exception e)
            {
                throw new ProtocolBufferParseException(e.Message);
            }
        }
    }
}
