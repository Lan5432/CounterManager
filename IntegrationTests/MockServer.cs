using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System.Net;
using WireMock;
using WireMock.Matchers.Request;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace IntegrationTests {
    public class MockServer : IDisposable {
        
        private readonly WireMockServer _server;
        private readonly IDictionary<MockEntry, string> _mockEntries;
        private readonly IDictionary<HttpMethod, Func<IRequestBuilder, IRequestBuilder>> methodToConfigureFunction =
            new Dictionary<HttpMethod, Func<IRequestBuilder, IRequestBuilder>> {
                { HttpMethod.Get, (requestMatcher) => requestMatcher.UsingGet() },
                { HttpMethod.Post, (requestMatcher) => requestMatcher.UsingPost() },
                { HttpMethod.Put, (requestMatcher) => requestMatcher.UsingPut() },
                { HttpMethod.Delete, (requestMatcher) => requestMatcher.UsingDelete() }
            };

        public MockServer(int port) {
            _server = WireMockServer.Start(port);
            _mockEntries = new Dictionary<MockEntry, string>();
        }

        public void MockResponse(string path, object data, HttpMethod method, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            var requestMatcher = Request.Create().WithPath(path);
            requestMatcher = methodToConfigureFunction[method].Invoke(requestMatcher);
            var body = JsonConvert.SerializeObject(data);
            var guid = ComputeGuid(path, method);

            _server.Given(requestMatcher)
                .WithGuid(guid)
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(statusCode)
                        .WithHeader(HeaderNames.Accept, System.Net.Mime.MediaTypeNames.Application.Json)
                        .WithBody(body)
                );
        }

        private string ComputeGuid(string path, HttpMethod method)
        {
            var mockEntry = new MockEntry(path, method);
            string guid;
            if(!_mockEntries.ContainsKey(mockEntry)) {
                guid = Guid.NewGuid().ToString();
                _mockEntries.Add(mockEntry, guid);
            } else {
                guid = _mockEntries[mockEntry];
            }
            return guid;
        }

        public IRequestMessage GetRequestEntry(string path, HttpMethod requestMethod)
        {
            var entry = _server.LogEntries.LastOrDefault(
                entry => entry.RequestMessage.Path.Contains(path) && entry.RequestMessage.Method.Equals(requestMethod.Method)
            );
            if(entry == null) {
                throw new ArgumentException("No request found for these parameters");
            }
            return entry.RequestMessage;
        }


        public void Dispose()
        {
            _server.Stop();
            _server.Dispose();
        }
    }
}
