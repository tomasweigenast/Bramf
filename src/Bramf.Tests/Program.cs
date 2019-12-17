using Bramf.Configuration;
using System;

namespace Bramf.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            ConfigurationEnvironment configuration = new ConfigurationEnvBuilder(Environment.GetFolderPath(Environment.SpecialFolder.Desktop))
                .AddProvider<Entity>()
                .AddProvider<EntityEncrypted>(x =>
                {
                    x.Encrypt = true;
                    x.Load = false;
                })
                .Build();

            Console.WriteLine($"Value: {configuration.Get<EntityEncrypted>().Id}");

            configuration.BeginEdit<EntityEncrypted>(x =>
            {
                x.Id = "hello world";
            });

            Console.WriteLine($"Value: {configuration.Get<EntityEncrypted>().Id}");

            Console.ReadLine();
        }
    }

    [Config]
    public class Entity
    {
        public string Id { get; set; } = "Mi id";

        public bool Enabled { get; set; }
    }

    [Config]
    public class EntityEncrypted
    {
        public string Id { get; set; } = "Mi id";

        public bool Enabled { get; set; }
    }
}
