using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Bson;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using System;

namespace WorkerService;
public class WorkerController : BackgroundService
{
    private ILogger<WorkerController> _ilogger;
    private IConfiguration _config;
    private IMongoDatabase database;
    private IMongoCollection<Customer> customerCollection;
    private IMongoCollection<Product> productCollection;

     public WorkerController(ILogger<WorkerController> logger, IConfiguration config)
    {
        _ilogger = logger;
        _config = config;
        var hostName = System.Net.Dns.GetHostName(); 
        var ips = System.Net.Dns.GetHostAddresses(hostName); 
        var _ipaddr = ips.First().MapToIPv4().ToString(); 
        _ilogger.LogDebug(1, $"**********Worker responding from {_ipaddr}**********"); 
        //MongoClient dbClient = new MongoClient(_config["MongoDBConct"]);
        MongoClient dbClient = new MongoClient("mongodb+srv://auktionshus:jamesbond@auktionshus.aeg6tzo.mongodb.net/test");
        database = dbClient.GetDatabase("Auktionshus");
        customerCollection = database.GetCollection<Customer>("User");
        productCollection = database.GetCollection<Product>("Product");
    }
    //Sequential convoy
     public void Receivebid(){
        _ilogger.LogDebug($"**********Starting bid**********");
        var factory = new ConnectionFactory() { HostName = "backend",DispatchConsumersAsync = true };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
          _ilogger.LogDebug($"**********Processing bid from**********");
            channel.QueueDeclare(queue: "products",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            //var consumer = new EventingBasicConsumer(channel);
            var consumer = new AsyncEventingBasicConsumer(channel);
                consumer.Received += async (model, ea) =>
                {
                  _ilogger.LogDebug($"**********Processing bid{ea}**********");
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Productdto? dto = JsonSerializer.Deserialize<Productdto>(message);

                      if (dto != null)
                        {
                            _ilogger.LogInformation($"**********Products: {dto.Id} offerings {dto.Price} **********");
                            await validate(dto.Id,dto.Price);
                        } 
                        else 
                          {
                            _ilogger.LogWarning($"Could not deserialize message with body: {message}");
                          }
                    _ilogger.LogInformation($"**********data recieved: {message}**********");
                };

            channel.BasicConsume(queue: "products",
                                 autoAck: true,
                                 consumer: consumer);

            Console.WriteLine("awaiting bid...");
   }
  }

  public async Task validate(string? id, double? price)
  {
    Product product = productCollection.Find(c => c.Id.Equals(id)).FirstOrDefault();
    if(price > product.Price)
    {
      _ilogger.LogInformation("**********A bid has been made**********");
      product.Price = price;
      productCollection.ReplaceOne(a => a.Id.Equals(id),product);
      _ilogger.LogInformation($"id: {id}, price: {price}");
    }
    else 
    {
      _ilogger.LogInformation("**********Your bid is too low**********");
    }
  } 

  protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      while(!stoppingToken.IsCancellationRequested)
      {
        try{
          _ilogger.LogInformation("**********Connection made**********");
             Receivebid();
          
        }
        catch(Exception ex){
          _ilogger.LogDebug("**********Connection failed**********");
        }
        await Task.Delay(1000, stoppingToken);
      }
      
    }

}
