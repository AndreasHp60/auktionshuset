using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Bson;

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
  public void ChangeState( string id, int state, double price)
  {
    _ilogger.LogInformation("State has been changed:");
    Product product = productCollection.Find(c => c.Id.Equals(id)).FirstOrDefault();
    product.State = state;
    product.Price = price;
    if(product.State == 3)
    {
        product.Time = DateTime.Now;
    }
    productCollection.ReplaceOne(a => a.Id.Equals(id),product);
  }


//   public async Task TimeCheck(Product product)
//   {
//     if(product.Price>(410))
//     Sold();
//   }

// [HttpPut("Sold")]
//   public void Sold()
//   {
//      _ilogger.LogInformation("This product has been sold:");
//     Product product = productCollection.Find(p => p.State.Equals(3)).FirstOrDefault();
//     product.State = 4;
//     productCollection.ReplaceOne(a => a.State.Equals(3),product);
//   }

}
