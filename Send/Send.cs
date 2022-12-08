using System;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace productSender;

class Send
{
    public static void Main()
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: "products",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var vare = new Product
            (
                "Lampe",
                "flot lampe",
                "lys",
                20,
                399,
                5,
                DateTime.Parse("2020-05-18T14:10:30Z"),
                "Billede?.png",
                9
            );
            string message = "Product sendt til databasen.";
            var body = JsonSerializer.SerializeToUtf8Bytes(vare);

            channel.BasicPublish(exchange: "",
                                 routingKey: "products",
                                 basicProperties: null,
                                 body: body);
            Console.WriteLine(" [x] Sent {0}", vare);
        }

        Console.WriteLine(" Press [enter] to exit.");
        Console.ReadLine();
        
    }
}