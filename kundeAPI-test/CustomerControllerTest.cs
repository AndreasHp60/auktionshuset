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
    readonly CustomerController controller;


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
    public async Task TestCreateCustomerEmptyPassword()
    {
        Customer customer = new Customer()
            {
                FirstName = "Test firstname",
                LastName = "Test lastname",
                Password = "",
                Email = "Test email",
                Phonenr = "Test phonenr",
                Address ="Test Address",
                Postal = 2020,
                City = "Test city",
                Country = "Test country"
            };
        // Arrange
        var mockRepo = new Mock<ICustomerRepositoryService>().Object;     
        var controller = new CustomerController(_logger, _configuration, mockRepo);

        // Act        
        var result = await controller.CreateCustomer(customer);
        
        // Assert
        Assert.IsNull(result);
    }

    [Test]
    public async Task TestCreateCustomerNullPassword()
    {
        Customer customer = new Customer()
            {
                FirstName = "Test firstname",
                LastName = "Test lastname",
                Password = null,
                Email = "Test email",
                Phonenr = "Test phonenr",
                Address ="Test Address",
                Postal = 2020,
                City = "Test city",
                Country = "Test country"
            };
        // Arrange
        var mockRepo = new Mock<ICustomerRepositoryService>().Object;     
        var controller = new CustomerController(_logger, _configuration, mockRepo);

        // Act        
        var result = await controller.CreateCustomer(customer);
        
        Console.WriteLine($"customertest: {result}");
        // Assert
        Assert.IsNull(result);
        
    }
}