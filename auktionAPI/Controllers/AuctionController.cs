using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Bson;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace AuctionService.Controllers;

[ApiController]
[Route("[controller]")]
public class AuctionController : ControllerBase
{
    private List<Product> _products = new List<Product>();
    private List<Customer> _customer = new List<Customer>();
    private readonly ILogger<AuctionController> _ilogger;
    private readonly IConfiguration _config;
    private readonly IMongoDatabase database;
    private readonly IMongoCollection<Customer> customerCollection;
    private readonly IMongoCollection<Product> productCollection;
    private string _nextID;

     public AuctionController(ILogger<AuctionController> logger, IConfiguration config)
    {
        _ilogger = logger;
        _config = config;
        //MongoClient dbClient = new MongoClient(_config["MongoDBConct"]);
        MongoClient dbClient = new MongoClient("mongodb+srv://auktionshus:jamesbond@auktionshus.aeg6tzo.mongodb.net/test");
        database = dbClient.GetDatabase("Auktionshus");
        customerCollection = database.GetCollection<Customer>("User");
        productCollection = database.GetCollection<Product>("Product");
    }

 [HttpGet("GetProductsByState")] 
  public List <Product> GetProductsByState(int getState) 
  { 
    _ilogger.LogInformation($"Products with state {getState} has been fetched");
    var productDocument = productCollection.Find(new BsonDocument()).ToList();
    productDocument.ToJson();
    return productDocument.Where(a => a.State.Equals(getState)).ToList();
  }

  [HttpPut("ChangeState")]
  public void ChangeState( string id, int state, double price, int durationDays)
  {
    _ilogger.LogInformation("State has been changed");
    Product product = productCollection.Find(c => c.Id.Equals(id)).FirstOrDefault();
    product.State = state;
    product.Price = price;
    if(product.State == 3)
    {
        _ilogger.LogInformation("Auction has started!");
        product.Time = DateTime.Now.AddDays(durationDays);
    }
    productCollection.ReplaceOne(a => a.Id.Equals(id),product);
  }

    [HttpPut("MakeBid")]
  public void MakeBid( string id,  double price)
  { 
    Product product = productCollection.Find(c => c.Id.Equals(id)).FirstOrDefault();
    if(price > product.Price)
    {
      _ilogger.LogInformation("A bid has been made");
      product.Price = price;
      productCollection.ReplaceOne(a => a.Id.Equals(id),product);
    }
    else {
      product.Price = product.Price;
      _ilogger.LogInformation("Your bid is too low");
    }
    
  }

  [HttpPost("Sendbid")]
  public void Sendbid(Productdto product){
        
          var factory = new ConnectionFactory() { HostName = "localhost" };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: "products",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var body = JsonSerializer.SerializeToUtf8Bytes(product);

            channel.BasicPublish(exchange: "",
                                 routingKey: "products",
                                 basicProperties: null,
                                 body: body);
            Console.WriteLine(" [x] Sent {0}");
        }

        Console.WriteLine(" Press [enter] to exit.");
  }

  [HttpGet("Getbid")]

  public void Receivebid(){
        _ilogger.LogDebug($"Starting bid");
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
          _ilogger.LogDebug($"Processing bid from  ");
            channel.QueueDeclare(queue: "products",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                  _ilogger.LogDebug($"Processing bid");
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Productdto? dto = JsonSerializer.Deserialize<Productdto>(message);

                      if (dto != null)
                        {
                            _ilogger.LogInformation($"Processing bid {dto.Id} from  ");

                            MakeBid(dto.Id, dto.Price);
                            
                        } else {
                            _ilogger.LogWarning($"Could not deserialize message with body: {message}");
                        }
                    Console.WriteLine(" [x] Received {0}", message);
                };

            channel.BasicConsume(queue: "products",
                                 autoAck: true,
                                 consumer: consumer);

            Console.WriteLine("bid complet");
   }
  }
}
