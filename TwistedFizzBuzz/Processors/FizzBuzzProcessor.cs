using System.Text;
using TwistedFizzBuzz.Interfaces;

namespace TwistedFizzBuzz.Processors
{
    public class FizzBuzzProcessor
    {
        private readonly IFizzBuzzApiClient _apiClient;

        public FizzBuzzProcessor(IFizzBuzzApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public IEnumerable<string> GenerateFizzBuzz()
        {
            return GenerateFizzBuzz(1, 100);
        }

        public IEnumerable<string> GenerateFizzBuzz(int start, int end, Dictionary<int, string>? customRules = null)
        {
            var firstValue = start <= end ? start : end;
            var lastValue = start <= end ? end : start;

            for (int i = firstValue; i <= lastValue; i++)
            {
                string output = GetFizzBuzzOutput(i, customRules);
                yield return output;
            }
        }

        public IEnumerable<string> GenerateFizzBuzz(List<int> numbers, Dictionary<int, string>? customRules = null)
        {
            foreach (var number in numbers)
            {
                string output = GetFizzBuzzOutput(number, customRules);
                yield return output;
            }
        }

        public async Task<IEnumerable<string>> GenerateFizzBuzzFromApiAsync(int start, int end)
        {
            var customRule = await _apiClient.GetFizzBuzzRuleFromApiAsync();

            return GenerateFizzBuzz(start, end, customRule);
        }

        public async Task<IEnumerable<string>> GenerateFizzBuzzFromApiAsync(List<int> numbers)
        {
            var customRule = await _apiClient.GetFizzBuzzRuleFromApiAsync();

            return GenerateFizzBuzz(numbers, customRule);
        }

        private string GetFizzBuzzOutput(int number, Dictionary<int, string>? rules)
        {
            if (rules == null || rules.Count == 0)
            {
                rules = new Dictionary<int, string>
                {
                    {3, "Fizz"},
                    {5, "Buzz"}
                };
            }

            StringBuilder output = new();

            foreach (var rule in rules)
            {
                if (number % rule.Key == 0)
                {
                    output.Append(rule.Value);
                }
            }

            if(output.Length == 0)
            {
                return number.ToString();
            }

            return output.ToString();
        }
    }
}
