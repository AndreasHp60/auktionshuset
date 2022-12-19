namespace WorkerService;

/// <summary>
/// Interface definition for the DB service to access the catalog data.
/// </summary>
public interface IWorkerRepositoryService
{
    void Receivebid();
    Task validate(string? id, double? price);
    Task CreateProduct(CancellationToken stoppingToken);

}