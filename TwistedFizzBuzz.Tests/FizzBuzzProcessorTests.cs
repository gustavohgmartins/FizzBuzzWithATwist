using Moq;
using TwistedFizzBuzz.Interfaces;
using TwistedFizzBuzz.Processors;

namespace TwistedFizzBuzz.Tests
{
    public class FizzBuzzProcessorTests
    {
        private readonly FizzBuzzProcessor _processor;
        private readonly Mock<IFizzBuzzApiClient> _apiClientMock;

        public FizzBuzzProcessorTests()
        {
            _apiClientMock = new Mock<IFizzBuzzApiClient>();
            _processor = new FizzBuzzProcessor(_apiClientMock.Object);
        }

        [Fact]
        public void GenerateFizzBuzz_DefaultRules_ShouldReturnCorrectValues()
        {
            var result = _processor.GenerateFizzBuzz().ToList();

            Assert.Equal("1", result[0]);
            Assert.Equal("Fizz", result[2]);
            Assert.Equal("Buzz", result[4]);
            Assert.Equal("FizzBuzz", result[14]);
            Assert.Equal("Fizz", result[98]);
            Assert.Equal("Buzz", result[99]);
        }

        [Fact]
        public void GenerateFizzBuzz_FromRange_ShouldReturnCorrectValues()
        {
            var result = _processor.GenerateFizzBuzz(-1, 101).ToList();

            Assert.Equal("-1", result[0]);
            Assert.Equal("Fizz", result[4]);
            Assert.Equal("Buzz", result[6]);
            Assert.Equal("FizzBuzz", result[16]);
            Assert.Equal("101", result.Last());
        }

        [Fact]
        public void GenerateFizzBuzz_FromRangeBackwards_ShouldReturnCorrectValues()
        {
            var result = _processor.GenerateFizzBuzz(1, -101).ToList();

            Assert.Equal("-101", result[0]);
            Assert.Equal("Buzz", result[1]);
            Assert.Equal("Fizz", result[2]);
            Assert.Equal("FizzBuzz", result[86]);
            Assert.Equal("1", result.Last());
        }

        [Fact]
        public void GenerateFizzBuzz_WithCustomRules_ShouldApplyRulesCorrectly()
        {
            var customRules = new Dictionary<int, string>
            {
                {4, "Alpha"},
                {6, "Beta"}
            };

            var result = _processor.GenerateFizzBuzz(1, 20, customRules).ToList();

            Assert.Equal("1", result[0]);
            Assert.Equal("Alpha", result[3]);
            Assert.Equal("Beta", result[5]);
            Assert.Equal("AlphaBeta", result[11]);
        }

        [Fact]
        public void GenerateFizzBuzz_FromList_ShouldReturnCorrectValues()
        {
            var numbers = new List<int> { 3, 5, 300, 7, 10 };

            var result = _processor.GenerateFizzBuzz(numbers).ToList();

            Assert.Equal(["Fizz", "Buzz", "FizzBuzz", "7", "Buzz"], result);
        }

        [Fact]
        public async Task GenerateFizzBuzzFromApiAsync_ShouldApplyApiRules()
        {
            var apiRules = new Dictionary<int, string>
            {
                {2, "Alpha"},
                {3, "Beta"}
            };

            _apiClientMock.Setup(c => c.GetFizzBuzzRuleFromApiAsync()).ReturnsAsync(apiRules);

            List<string> resultList = [];

            foreach (var output in await _processor.GenerateFizzBuzzFromApiAsync(1, 6))
            {
                resultList.Add(output);
            };

            Assert.Equal(["1", "Alpha", "Beta", "Alpha", "5", "AlphaBeta"], resultList);
        }
    }
}
