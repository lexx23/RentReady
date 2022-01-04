using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Primitives;
using Moq;

namespace RentReady.Tests.Common
{
    public class HttpRequestMock
    {
        private readonly Mock<HttpRequest> _requestMock;

        public HttpRequest Request => _requestMock.Object;

        public HttpRequestMock()
        {
            _requestMock = new Mock<HttpRequest>();
        }

        public HttpRequestMock AddQuery(Dictionary<string, StringValues> query)
        {
            if(query != null)
                _requestMock.Setup(req => req.Query).Returns(new QueryCollection(query));
            
            return this;
        }

        public HttpRequestMock AddBody(string body)
        {
            if (string.IsNullOrEmpty(body)) 
                return this;
            
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(body);
            writer.Flush();
            stream.Position = 0;
            _requestMock.Setup(req => req.Body).Returns(stream);

            return this;
        }
        
    }
}