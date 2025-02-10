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
            var result = fizzBuzzProcessor.GenerateFizzBuzz();

            foreach (var item in result)
            {
                Console.WriteLine(item);
            }

            Console.ReadLine();
        }
    }
}