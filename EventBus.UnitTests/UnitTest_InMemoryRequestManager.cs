using FPTS.FIT.BDRD.BuildingBlocks.EventBus;
using System.Diagnostics;
using Xunit;
using Xunit.Abstractions;

namespace EventBus.UnitTests
{
    public class UnitTest_InMemoryRequestManager
    {
        private readonly InMemoryRequestManager _requestManager;
        private readonly ITestOutputHelper _output;

        public UnitTest_InMemoryRequestManager(ITestOutputHelper output)
        {
            _requestManager = new InMemoryRequestManager();
            _output         = output;
        }

        [Fact]
        public void Test_GetResponsAsync_Timeout()
        {
            Stopwatch stopwatch = new();
            stopwatch.Start();
            var res = _requestManager.GetResponseAsync("123", 100).Result;
            stopwatch.Stop();
            _output.WriteLine(stopwatch.Elapsed+"");
            Assert.Null(res);
        }
    }
}