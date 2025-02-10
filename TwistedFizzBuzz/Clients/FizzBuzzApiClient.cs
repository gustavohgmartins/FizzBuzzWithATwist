using System.Text.Json;
using Polly;
using Polly.Retry;
using TwistedFizzBuzz.Interfaces;
using TwistedFizzBuzz.Models;

namespace TwistedFizzBuzz.Clients
{
    public class FizzBuzzApiClient : IFizzBuzzApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly AsyncRetryPolicy<HttpResponseMessage> _retryPolicy;
        private readonly string _fizzBuzzApiUrl = "https://pie-healthy-swift.glitch.me/word";

        public FizzBuzzApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _retryPolicy = Policy
                .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                .Or<HttpRequestException>()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        public async Task<Dictionary<int, string>> GetFizzBuzzRuleFromApiAsync()
        {
            Console.WriteLine($"Attempting to connect to the FizzBuzz API at {_fizzBuzzApiUrl}...");

            var response = await _retryPolicy.ExecuteAsync(() => _httpClient.GetAsync(_fizzBuzzApiUrl));
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var responseObj = JsonSerializer.Deserialize<FizzBuzzApiResponse>(responseString);
            var rule = new Dictionary<int, string>();

            if (responseObj is not null)
            {
                rule[responseObj.number] = responseObj.word;
                Console.WriteLine("The API response rule has been successfully fetched.");
            }
            else
            {
                Console.WriteLine("The API response rule is empty.");
            }

            Console.WriteLine("Number: " + responseObj?.number);
            Console.WriteLine("Word: " + responseObj?.word);

            return rule;
        }
    }
}
