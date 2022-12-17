using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using CustomerService;

namespace CustomerService;

/// <summary>
/// MongoDB database context class.
/// </summary>
public class MongoDBContext
{
    private ILogger<MongoDBContext> _ilogger;
    private IConfiguration _config;
    public IMongoDatabase Database { get; set; }
    public IMongoCollection<Customer> Collection { get; set; }

    /// <summary>
    /// Create an instance of the context class.
    /// </summary>
    /// <param name="logger">Global logging facility.</param>
    /// <param name="config">System configuration instance.</param>
    public MongoDBContext(ILogger<MongoDBContext> logger, IConfiguration config)
    {
        _ilogger = logger;
        _config = config;
        
        BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));

        // VI BRUGER IKKE CONFIG!!
        
        // var client = new MongoClient(_config["MongoDBConct"]);
        // Database = client.GetDatabase(_config["DatabaseName"]);
        // Collection = Database.GetCollection<Customer>("Collection");

        var client = new MongoClient("mongodb+srv://auktionshus:jamesbond@auktionshus.aeg6tzo.mongodb.net/test");
        Database = client.GetDatabase("Auktionshus");
        Collection = Database.GetCollection<Customer>("User");

        // MongoClient dbClient = new MongoClient("mongodb+srv://auktionshus:jamesbond@auktionshus.aeg6tzo.mongodb.net/test");
        // database = dbClient.GetDatabase("Auktionshus");
        // customerCollection = database.GetCollection<Customer>("User");
        
        logger.LogInformation($"Connected to database {Database}");
        logger.LogInformation($"Using collection {Collection}");
    }

}
