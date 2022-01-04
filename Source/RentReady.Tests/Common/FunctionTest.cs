using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace RentReady.Tests.Common
{
    public abstract class FunctionTest
    {
        protected readonly ILogger Logger;
        protected readonly HttpRequestMock RequestMock;

        protected FunctionTest()
        {
            RequestMock = new HttpRequestMock();
            Logger = NullLoggerFactory.Instance.CreateLogger("Null Logger");
        }
    }
}