using Microsoft.Extensions.DependencyInjection;
using System;

namespace Bramf.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .BuildServiceProvider();

            Console.ReadLine();
        }
    }
}