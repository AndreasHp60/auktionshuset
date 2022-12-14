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

  [HttpPost("Sendbid")]
  public void Sendbid(string idd, double pricee){
        var productdto = new Productdto(){
          Id = idd,
          Price = pricee
        };
        var factory = new ConnectionFactory() { HostName = "rabbitmq-dev" };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        
        {
            channel.QueueDeclare(queue: "products",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var body = JsonSerializer.SerializeToUtf8Bytes(productdto);
            _ilogger.LogInformation("Information serialized");

            channel.BasicPublish(exchange: "",
                                 routingKey: "products",
                                 basicProperties: null,
                                 body: body);
            _ilogger.LogInformation("Information send");
            Console.WriteLine(" [x] Sent {0}");
        }

        Console.WriteLine(" Press [enter] to exit.");
  } 
}
