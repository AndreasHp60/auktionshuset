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
    private readonly ICustomerRepositoryService _repository;

    public CustomerController(ILogger<CustomerController> logger, 
    IConfiguration config, ICustomerRepositoryService repository)
    {
        _ilogger = logger;
        _config = config;
        _repository = repository;
        var hostName = System.Net.Dns.GetHostName(); 
        var ips = System.Net.Dns.GetHostAddresses(hostName); 
        var _ipaddr = ips.First().MapToIPv4().ToString(); 
        _ilogger.LogDebug(1, $"**********CustomerController responding from {_ipaddr}**********");

        // //MongoClient dbClient = new MongoClient(_config["MongoDBConct"]);
        // MongoClient dbClient = new MongoClient("mongodb+srv://auktionshus:jamesbond@auktionshus.aeg6tzo.mongodb.net/test");
        // database = dbClient.GetDatabase("Auktionshus");
        // collection = database.GetCollection<Customer>("User");
        // export MongoDBConct=mongodb+srv://auktionshus:jamesbond@auktionshus.aeg6tzo.mongodb.net/test
    }

[HttpGet("GetCustomers")] 
public async Task<List<Customer>> Get() 
  {
      var result = await _repository.GetCustomers();
      _ilogger.LogInformation("**********Customers fetched**********");
      return result;
  }

[HttpGet("GetCustomerById")] 
public async Task<Customer?> GetCustomer(string id) 
  { 
    _ilogger.LogInformation($"**********Customer fetched by email**********");
    var result = await _repository.GetCustomer(id);
    result.ToJson();
    return result;
  }

[HttpPost("createCustomer")]
public async Task<Customer?> CreateCustomer( Customer customer )
  {

      Console.WriteLine($"customer: {customer.Email}");
      if(string.IsNullOrEmpty(customer.Email))
      {
        return null;
      }
      
      var result = await _repository.CreateCustomer(customer);
      _ilogger.LogInformation($"**********Customer{customer.FirstName} created:**********");
      return result;
  }

[HttpPut("updateCustomer")]
public async Task<Customer> updateCustomer( Customer customer)
  {
       var result = await _repository.updateCustomer(customer);
      _ilogger.LogInformation($"**********Customer{customer.Email} have been updated:**********");
      return result;
  }

[HttpDelete("deleteCustomer")]
public async Task<Customer> deleteCustomer(string id, Customer customer)
  {
    var result = await _repository.deleteCustomer(id, customer);
    return result;
  }
}
