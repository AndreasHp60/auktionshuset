using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Bson;

namespace CustomerService.Controllers;

[ApiController]
[Route("[controller]")]
public class CustomerController : ControllerBase
{
    private List<Customer> _customers = new List<Customer>();
    private readonly ILogger<CustomerController> _ilogger;
    private readonly IConfiguration _config;
    private readonly IMongoDatabase database;
    private readonly IMongoCollection<Customer> collection;

    public CustomerController(ILogger<CustomerController> logger, IConfiguration config)
    {
        _ilogger = logger;
        _config = config;
        var hostName = System.Net.Dns.GetHostName(); 
        var ips = System.Net.Dns.GetHostAddresses(hostName); 
        var _ipaddr = ips.First().MapToIPv4().ToString(); 
        _ilogger.LogInformation(1, $"**********CustomerController responding from {_ipaddr}**********");

        //MongoClient dbClient = new MongoClient(_config["MongoDBConct"]);
        MongoClient dbClient = new MongoClient("mongodb+srv://auktionshus:jamesbond@auktionshus.aeg6tzo.mongodb.net/test");
        database = dbClient.GetDatabase("Auktionshus");
        collection = database.GetCollection<Customer>("User");
    }

[HttpGet("GetCustomers")] 
public List <Customer> Get() 
  { 
    _ilogger.LogInformation("**********Customers fetched**********");
    var document = collection.Find(new BsonDocument()).ToList();
    document.ToJson();
    return document.ToList();
  }

[HttpGet("GetCustomerByEmail")] 
public Customer GetByEmail(string customerEmail) 
  { 
    _ilogger.LogInformation($"**********Customer fetched by email**********");
    var document = collection.Find(new BsonDocument()).ToList();
    document.ToJson();
    return document.Where(c => c.Email.Equals(customerEmail)).First();
  }

[HttpPost("createcustomer")]
public void CreateCustomer(Customer customer)
  {
    _ilogger.LogInformation($"**********Customer{customer.FirstName} created:**********");
      var newCustomer = customer;
      collection.InsertOne(newCustomer);
  }

[HttpPut("updateCustomer")]
public void updateCustomer(Customer customer,string customerEmail)
  {
    //hvis ikke man sletter eller ændrer id crasher den!
    _ilogger.LogInformation($"**********Customer{customer.Email} have been updated:**********");
    var newCustomer = customer;
    customer = collection.Find(c => c.Email.Equals(customerEmail)).FirstOrDefault();
    collection.ReplaceOne(c => c.Email.Equals(customerEmail),newCustomer);
  }

[HttpDelete("deleteCustomer")]
public void deleteCustomer(string customerEmail)
  {
    _ilogger.LogInformation($"**********Customer{customerEmail} have been deleted:**********");
    collection.DeleteOne(c => c.Email.Equals(customerEmail));
  }
}
