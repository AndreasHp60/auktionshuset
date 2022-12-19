namespace ProductService;

/// <summary>
/// Interface definition for the DB service to access the catalog data.
/// </summary>
public interface ICustomerRepositoryService
{
    Task<List<Product>> GetProduct();
    Task<Product> GetProductByEmail(string customerEmail);
    void CreateProduct(Product product, string customerEmail, string name, string description, string category, double assesment, double minbid);
    void updateProduct( string productId, string name, string description, string category, double assesment, double price, double minbid, short state);
    void deleteProduct(string id);

}