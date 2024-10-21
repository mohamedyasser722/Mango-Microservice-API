namespace Mango.Service.ProductAPI.Contracts.Product;

public record ProductResponse
(
    int ProductId,
    string Name,
    double Price,
    string Description,
    string CategoryName,
    string? ImageUrl,
    string? ImageLocalPath
);