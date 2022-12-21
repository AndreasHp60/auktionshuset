using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Bson;

namespace ProductService.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private List<Product> _products = new List<Product>();

    private List<Customer> _customer = new List<Customer>();
    private readonly ILogger<ProductController> _ilogger;
    private readonly IConfiguration _config;
    private readonly IMongoDatabase database;
    private readonly IMongoCollection<Customer> customerCollection;
    private readonly IMongoCollection<Product> productCollection;

  public ProductController(ILogger<ProductController> logger, IConfiguration config)
    {
        _ilogger = logger;
        _config = config;
        var hostName = System.Net.Dns.GetHostName(); 
        var ips = System.Net.Dns.GetHostAddresses(hostName); 
        var _ipaddr = ips.First().MapToIPv4().ToString(); 
        _ilogger.LogDebug(1, $"**********ProductController responding from {_ipaddr}**********");
        //MongoClient dbClient = new MongoClient(_config["MongoDBConct"]);
        MongoClient dbClient = new MongoClient("mongodb+srv://auktionshus:jamesbond@auktionshus.aeg6tzo.mongodb.net/test");
        database = dbClient.GetDatabase("Auktionshus");
        customerCollection = database.GetCollection<Customer>("User");
        productCollection = database.GetCollection<Product>("Product");
    }

 [HttpGet("GetProducts")] 
  public List <Product> Get() 
  { 
    _ilogger.LogInformation("**********Products fetched:**********");
    var productDocument = productCollection.Find(new BsonDocument()).ToList();
    productDocument.ToJson();
    return productDocument.ToList();
  }

  [HttpGet("GetProductByEmail")] 
  public Product GetProductByEmail(string customerEmail) 
  { 
    _ilogger.LogInformation($"**********Product fetched by:{customerEmail}**********");
    var productDocument = productCollection.Find(new BsonDocument()).ToList();
    productDocument.ToJson();
    return productDocument.ToList().Where(c => c.customer.Email.Equals(customerEmail)).FirstOrDefault();
  }

  //skal testes i working system om den virker
  [HttpPost("CreateProduct")]
  public void CreateProduct( [FromBody]Product product, [FromQuery]string customerEmail/*, string name, string description, string category, double assesment, double minbid*/)
  {
      _ilogger.LogInformation($"**********Product{product.Name} has been created:**********");
      Customer customers = customerCollection.Find(c => c.Email.Equals(customerEmail)).FirstOrDefault();
      
      /*product = new Product(){
      Name = name, 
      Description = description, 
      Category = category, 
      Assesment = assesment, 
      Price = 0, 
      MinBid = minbid,
      State = 1, 
      customer = customers};*/
      
      product.customer=customers;
      productCollection.InsertOne(product);
  }

[HttpPut("updateProduct")]
  public void updateProduct( string productId, string name, string description, string category, double assesment, double price, double minbid, short state)
  {
    Product product = productCollection.Find(c => c.Id.Equals(productId)).FirstOrDefault();
    product.Name = name;
    product.Price = price;
    product.Description = description;
    product.Category = category;
    product.Assesment = assesment;
    product.Price = price;
    product.MinBid = minbid;
    product.State = state;

    _ilogger.LogInformation($"**********Product{product.Name} have been updated:**********");

    productCollection.ReplaceOne(c => c.Id.Equals(productId),product);
  }

  [HttpDelete("deleteProduct")]
  public void deleteProduct( string id)
  {
    _ilogger.LogInformation($"**********Product{id} have been deleted:**********");
    productCollection.DeleteOne(c => c.Id.Equals(id));
  }

}