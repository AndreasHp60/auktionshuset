using Microsoft.AspNetCore.Mvc;
using CustomerService;
using System.Linq;
using MongoDB.Driver;

namespace CustomerService.Controllers;

[ApiController]
[Route("[controller]")]
public class CustomerController : ControllerBase
{
 private List<Customer> _customers = new List<Customer>() { 
      new Customer() {  
          Id = 1, 
          Name = "International Bicycles A/S", 
          Address1 = "Nydamsvej 8", 
          Address2 = null, 
          PostalCode = 8362, 
          City = "Hørning", 
          TaxNumber = "DK-75627732"
      }, 
            new Customer() {  
          Id = 2, 
          Name = "International Carcycles A/S", 
          Address1 = "Bergens 8", 
          Address2 = null, 
          PostalCode = 8210, 
          City = "Aarhus", 
          TaxNumber = "NN-75627732"
      } 
  }; 

    private readonly ILogger<CustomerController> _ilogger;
    private readonly IConfiguration _config;
    private readonly IMongoDatabase database;

    private readonly IMongoCollection<Customer> collection;

    public CustomerController(ILogger<CustomerController> logger, IConfiguration config)
    {
        _ilogger = logger;
        _config = config;
        MongoClient dbClient = new MongoClient("mongodb+srv://auktionshus:jamesbond@auktionshus.aeg6tzo.mongodb.net/test");
        database = dbClient.GetDatabase("Auktionshus");
        collection = database.GetCollection<Customer>("User");
    }

  [HttpGet("GetCustomers")] 
  public List <Customer> Get() 
  { 
    return _customers.ToList();
  }

  [HttpGet("GetCustomerById")] 
  public Customer GetByid(int customerId) 
  { 
    return _customers.Where(c => c.Id == customerId).First();
  }
}
