using System;

namespace Bramf.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            Func<Entity, string> obj = x => x.Id;
            Delegate[] list = obj.GetInvocationList();
            Type t = obj.GetType();

            Console.ReadLine();
        }
    }

    public class Entity
    {
        public string Id => Guid.NewGuid().ToString();
    }
}
