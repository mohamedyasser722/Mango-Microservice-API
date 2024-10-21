using Mango.Service.ProductAPI.Contracts.Product;
using Mango.Service.ProductAPI.Models;
using Mango.Services.ProductAPI.Abstraction.ResultErrorHandellingPattern;

namespace Mango.Service.ProductAPI.Services.IServices;

public interface IProductService
{
    // get All 
    Task<IEnumerable<ProductResponse>> GetAllProducts(CancellationToken cancellationToken = default);
    // get by id
    Task<Result<ProductResponse>> GetProductById(int productId, CancellationToken cancellationToken = default);
    // create Coupon
    Task<Result<Product>> CreateProduct(ProductRequest coupon, CancellationToken cancellationToken = default);
    // update Coupon
    Task<Result<ProductResponse>> UpdateProduct(int productId, ProductRequest coupon, CancellationToken cancellationToken = default);
    // delete Coupon
    Task<Result<bool>> DeleteProduct(int productId, CancellationToken cancellationToken = default);
}
