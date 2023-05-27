using H.Formatters;
using H.Pipes;
using MessagePack;

namespace Tests
{
    internal static class Program
    {
        static void Main()
        {
            var client = new PipeClient<SomeClass>("Test", formatter: new NewtonsoftJsonFormatter());
            client.MessageReceived += (o, args) => Console.WriteLine("MessageReceived: " + args.Message);
            client.Disconnected += (o, args) => Console.WriteLine("Disconnected from server");
            client.Connected += (o, args) =>
            {
                Console.WriteLine("Connected to server");
                client.WriteAsync(new SomeClass() { Text = "1" }).Wait();
            };
            client.ConnectAsync().Wait();

            client.WriteAsync(new SomeClass() { Text = "1" }).Wait();
            Console.ReadLine();

        }
    }
    [Serializable]
    public class SomeClass
    {
        //[Key(0)]
        //public Guid Id { get; set; } = Guid.NewGuid();
        public string? Text { get; set; }

        public override string ToString()
        {
            return $"\"{Text}\"";
        }
    }

}