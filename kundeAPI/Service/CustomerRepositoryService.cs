using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Linq;


namespace CustomerService;

/// <summary>
/// Interface definition for the DB service to access the catalog data.
/// </summary>
public interface ICustomerRepositoryService
{
    // void deleteCustomer(string id);
    // void updateCustomer(Customer customer);
    Task<Customer> CreateCustomer( Customer customer );
    Task<List<Customer>> GetCustomers();

    Task<Customer?> GetCustomer(string id);

    Task<Customer> updateCustomer( Customer customer);

    Task<Customer> deleteCustomer(string id, Customer customer);

    
    
}

/// <summary>
/// MongoDB repository service
/// </summary>
public class CustomerRepositoryService : ICustomerRepositoryService
{
    private ILogger<CustomerRepositoryService> _ilogger;
    private IConfiguration _config;
    private IMongoDatabase _database;
    private IMongoCollection<Customer?> _collection;

    /// <summary>
    /// Creates a new instance of the CatalogMongoDBService.
    /// </summary>
    /// <param name="logger">The commun logger facility instance</param>
    /// <param name="config">Systemm configuration instance</param>
    /// <param name="dbcontext">The database context to be used for accessing data.</param>
    public CustomerRepositoryService(ILogger<CustomerRepositoryService> logger, 
            IConfiguration config, MongoDBContext dbcontext)
    {
        _ilogger = logger;
        _config = config;
        _database = dbcontext.Database;        
        _collection = dbcontext.Collection;
    }

    /// <summary>
    /// Retrieves a product item based on its unique id.
    /// </summary>
    /// <param name="productId">The products unique id</param>
    /// <returns>The products item requested.</returns>
    public async Task<Customer?> GetCustomer(string id)
    {
        // Customer? product = null;
        // var filter = Builders<Customer>.Filter.Eq(x => x.Id, id);
        
        // try
        // {
        //     product = await _collection.Find(filter).SingleOrDefaultAsync();
        // }
        // catch(Exception ex)
        // {
        //     _ilogger.LogError(ex, ex.Message);
        // }

        // return product;    
    var result =  _collection.Find(new BsonDocument()).ToList();
    result.ToJson();
    return result.Where(c => c.Id.Equals(id)).First();
    }

    public async Task<List<Customer>> GetCustomers()
    {
        var result =  _collection.Find(new BsonDocument()).ToList();
        result.ToJson();
        return result.ToList();
    } 

    /// <summary>
    /// Add a new Product Item to the database.
    /// </summary>
    /// <param name="item">Product to add to the catalog/param>
    /// <returns>Product with updated Id</returns>
    public async Task<Customer> CreateCustomer(Customer customer)
    {
        
        await _collection.InsertOneAsync(customer);
        return customer;
    }

    public async Task<Customer> updateCustomer( Customer customer)
    {
        _ilogger.LogInformation($"**********Customer{customer.Id} have been updated:**********");
        customer = _collection.FindOneAndReplace(c => c.Id.Equals(customer.Id),customer);
        return customer;
    }

    public async Task<Customer> deleteCustomer(string id, Customer customer)
    {
        _ilogger.LogInformation($"**********Customer{id} have been deleted:**********");
        _collection.DeleteOne(c => c.Id.Equals(id));
        return customer;
        
    }

}
