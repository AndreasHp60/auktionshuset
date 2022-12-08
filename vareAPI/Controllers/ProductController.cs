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
        //MongoClient dbClient = new MongoClient(_config["MongoDBConct"]);
        MongoClient dbClient = new MongoClient("mongodb+srv://auktionshus:jamesbond@auktionshus.aeg6tzo.mongodb.net/test");
        database = dbClient.GetDatabase("Auktionshus");
        customerCollection = database.GetCollection<Customer>("User");
        productCollection = database.GetCollection<Product>("Product");
    }

 [HttpGet("GetProducts")] 
  public List <Product> Get() 
  { 
    _ilogger.LogInformation("Customers fetched:");
    var productDocument = productCollection.Find(new BsonDocument()).ToList();
    productDocument.ToJson();
    return productDocument.ToList();
  }

  [HttpGet("GetProductByEmail")] 
  public Product GetProductByEmail(string customerEmail) 
  { 
    _ilogger.LogInformation("Customer fetched:");
    var productDocument = productCollection.Find(new BsonDocument()).ToList();
    productDocument.ToJson();
    return productDocument.ToList().Where(c => c.customer.Email.Equals(customerEmail)).FirstOrDefault();
  }

  //skal tilrettes og optimeres.
  //finally works!
  //hvad skal kunne oprettes??
  [HttpPost("CreateProduct")]
  public void CreateProduct( Product product, string email, string name, string description, string category, double assesment, double minbid)
  {
      _ilogger.LogInformation($"Product{product.Name} has been created:");
      Customer customers = customerCollection.Find(c => c.Email.Equals(email)).FirstOrDefault();
      
      product = new Product(){
      Name = name, 
      Description = description, 
      Category = category, 
      Assesment = assesment, 
      Price = 0, 
      MinBid = minbid,
      State = 1, 
      customer = customers};
      
      productCollection.InsertOne(product);
  }

[HttpPut("updateProduct")]
  public void updateCustomer(Product product, string customerEmail)
  {
    //Hvis id er null fejler den
    //Hvis man ikke indsætter id fejler den
    //hvad skal man kunne opdatere?
    _ilogger.LogInformation($"Customer{product.customer.Email} have been updated:");
    var newProduct = product;
    product = productCollection.Find(c => c.customer.Email.Equals(customerEmail)).FirstOrDefault();
    productCollection.ReplaceOne(c => c.customer.Email.Equals(customerEmail),newProduct);
  }

  //tilret
  [HttpDelete("deleteProduct")]
  public void deleteCustomer(string customerEmail)
  {
    _ilogger.LogInformation($"Customer{customerEmail} have been deleted:");
    productCollection.DeleteOne(c => c.customer.Email.Equals(customerEmail));
  }


}