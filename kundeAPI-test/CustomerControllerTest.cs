using NUnit.Framework;
using Moq;
using CustomerService.Controllers;
using CustomerService;
using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace CustomerControllerTests;

public class CustomerControllerTests
{

    private ILogger<CustomerController>? _logger = null;
    private IConfiguration? _configuration = null;
    //readonly CustomerController controller;

    private Customer CreateCustomer(string? Id)
{
    var customer = new Customer()
    {
        Id = null,
        FirstName = "Test firstname",
        LastName ="Test lastname",
        Password ="Test password",
        Email = "Test email",
        Phonenr = "Test phonenr",
        Address ="Test Address",
        Postal = 2020,
        City = "Test city",
        Country = "Test country"
    };
    return customer;
}


    [SetUp]
    public void Setup()
    {
         _logger = new Mock<ILogger<CustomerController>>().Object;

            var myConfiguration = new Dictionary<string, string>
                    {
                        {"TaxaBookingBrokerHost", "http://testhost.local"}
                    };

         _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(myConfiguration)
            .Build();
    
    }

    [Test]
    public void Test1()
    {
        var customer = new Customer()
            {
                Id = "null",
                FirstName = "Test firstname",
                LastName ="Test lastname",
                Password ="Test password",
                Email = "Test email",
                Phonenr = "Test phonenr",
                Address ="Test Address",
                Postal = 2020,
                City = "Test city",
                Country = "Test country"
            };
        var newCustomer = customer;
        // Arrange
  {     //var _customer = new customer();
        //var customerDTO = CreateCustomer(customer);
        //var mockRepo = new Mock<CustomerController>();
        //mockRepo.Setup(svc => svc.CreateCustomer(customerDTO)).Returns(Task.CompletedTask);
        var controller = new CustomerController(_logger, _configuration);

        // Act        
        var result = controller.CreateCustomer(newCustomer);
        var testID = result.Email;
        // Assert
        Assert.AreEqual("Test email", testID);}
    
       /* try{
            CreateCustomer();
            Assert.IsTrue(true);
        }
        catch{
            Assert.IsTrue(false);
        }*/
    }
}