using System.ComponentModel.DataAnnotations;

namespace Mango.Service.ProductAPI.Contracts.Product;

public record ProductRequest
(
    string Name ,
    double Price ,
    string Description ,
    string CategoryName ,
    string? ImageUrl ,
    string? ImageLocalPath 
);