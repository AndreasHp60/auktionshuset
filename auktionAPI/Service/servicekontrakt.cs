namespace AuctionService;

/// <summary>
/// Interface definition for the DB service to access the catalog data.
/// </summary>
public interface IauktionAPIRepositoryService
{
    Task<List<Product>> GetProductsByState(int getState);
    void ChangeState(string id, int state, double price, int durationDays);
    void Sendbid(string idd, double pricee);

}