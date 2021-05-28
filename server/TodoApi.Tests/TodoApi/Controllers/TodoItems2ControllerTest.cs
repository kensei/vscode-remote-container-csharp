using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Google.Protobuf;
using NUnit.Framework;

namespace TodoApi.Tests.TodoApi.Controllers
{
    [TestFixture]
    class TodoItems2ControllerTest
    {
        private HttpClient _httpClient;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            TestContext.WriteLine("OneTimeSetup");
            var factory = new TodoApiWebApplicationFactory();
            _httpClient = factory.CreateClient();
        }

        public void Dispose()
        {
            if (_httpClient != null)
            {
                _httpClient.Dispose();
                _httpClient = null;
            }
        }

        [Test]
        public async Task GetTodoItemsTest()
        {
            var result = await _httpClient.GetAsync("/api/v2.0/TodoItems");
            Assert.IsTrue(result.IsSuccessStatusCode);
        }

        [Test]
        public async Task GetTodoItemsResponseTest()
        {
            var result = await _httpClient.GetAsync("/api/v2.0/TodoItems");
            
            var message = ConvertHttpResponseToByte(result);
            var response = Deserialize<Pb.Messages.Todo.TodosGetResponse>(message);
            TestContext.WriteLine("ErrorCode:" + response.Common.ErrorCode);
            Assert.IsTrue(response.Common.ErrorCode == 0);

            TestContext.WriteLine("count:" + response.TodoItems.Count);
            Assert.IsTrue(response.TodoItems.Count == 3);
            Assert.IsTrue(response.TodoItems[0].Title == "hoge");
            Assert.IsTrue(response.TodoItems[1].Title == "fuga");
            Assert.IsTrue(response.TodoItems[2].Title == "piyo");
        }

        private byte[] ConvertHttpResponseToByte(HttpResponseMessage response)
        {
            using (Stream stream = response.Content.ReadAsStream())
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    return ms.ToArray();
                }
            }
        }

        private T Deserialize<T>(byte[] data) where T : IMessage<T>, new()
        {
            var parser = new MessageParser<T>(() => new T());
            return parser.ParseFrom(new MemoryStream(data));
        }

        private Pb.Messages.CommonDataResponse GetCommonResponse<T>(T response) where T : IMessage<T>
        {
            var type = response.GetType();
 
            // 自動生成されたコードなので、リクレクションで取る
            var res = (Pb.Messages.CommonDataResponse)type.GetProperty("Common").GetValue(response);
 
            return res;
        }
    }
}