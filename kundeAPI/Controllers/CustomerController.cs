using Microsoft.AspNetCore.Mvc;
using CustomerService;
using System.Linq;

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

    public CustomerController(ILogger<CustomerController> logger)
    {
        _ilogger = logger;
    }

  [HttpGet(Name = "GetCustomerById")] 
  public Customer Get(int customerId) 
  { 
      return _customers.Where(c => c.Id == customerId).First(); 
  } 
  
}
