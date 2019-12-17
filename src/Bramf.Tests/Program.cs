using Bramf.Configuration;
using System;
using System.IO;

namespace Bramf.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            ConfigurationRoot configuration = new ConfigurationRootBuilder(Environment.GetFolderPath(Environment.SpecialFolder.Desktop))
                .AddProvider<Entity>()
                .AddProvider<EntityEncrypted>(x => x.Encrypt = true)
                .Build();

            var encrypted = configuration.Get<EntityEncrypted>();
            encrypted.Id = "Hola mundo";
            configuration.Save();

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
