using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using TwistedFizzBuzz.Clients;
using TwistedFizzBuzz.Interfaces;
using TwistedFizzBuzz.Processors;

class Program
{
    async static Task Main(string[] args)
    {
        var serviceProvider = new ServiceCollection()
           .AddHttpClient("FizzBuzzClient", client =>
           {
               client.DefaultRequestHeaders.Accept.Clear();
               client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("FizzBuzzAgent", "1.0"));
           })
           .Services
           .AddTransient<IFizzBuzzApiClient, FizzBuzzApiClient>(provider =>
           {
               var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
               return new FizzBuzzApiClient(httpClientFactory.CreateClient("FizzBuzzClient"));
           })
           .AddTransient<FizzBuzzProcessor>()
           .BuildServiceProvider();

        var fizzBuzzProcessor = serviceProvider.GetService<FizzBuzzProcessor>();

        if (fizzBuzzProcessor is not null)
        {
            
            foreach (var output in await fizzBuzzProcessor.GenerateFizzBuzzFromApiAsync(1, 1000))
            {
                Console.WriteLine(output);
            }
            Console.ReadLine();
        }
    }
}