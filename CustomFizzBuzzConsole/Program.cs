using Microsoft.Extensions.DependencyInjection;
using TwistedFizzBuzz.Clients;
using TwistedFizzBuzz.Interfaces;
using TwistedFizzBuzz.Processors;

class Program
{
    static void Main(string[] args)
    {
        var serviceProvider = new ServiceCollection()
            .AddHttpClient()
            .AddTransient<IFizzBuzzApiClient, FizzBuzzApiClient>()
            .AddTransient<FizzBuzzProcessor>()
            .BuildServiceProvider();

        var fizzBuzzProcessor = serviceProvider.GetService<FizzBuzzProcessor>();

        if (fizzBuzzProcessor is not null)
        {
            Dictionary<int, string> CustomTokens = new()
            {
                {5, "Fizz"},
                {9, "Buzz"},
                {27, "Bar"},
            };

            var result = fizzBuzzProcessor.GenerateFizzBuzz(-20, 127, CustomTokens);

            foreach (var item in result)
            {
                Console.WriteLine(item);
            }

            Console.ReadLine();
        }
    }
}