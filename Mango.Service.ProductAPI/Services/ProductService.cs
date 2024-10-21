using Mango.Service.ProductAPI.Abstraction.Errors;
using Mango.Service.ProductAPI.Contracts.Product;
using Mango.Service.ProductAPI.Data;
using Mango.Service.ProductAPI.Models;
using Mango.Service.ProductAPI.Services.IServices;
using Mango.Services.ProductAPI.Abstraction.ResultErrorHandellingPattern;
using Mango.Services.ProductAPI.Helpers;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Mango.Service.ProductAPI.Services;

public class ProductService(AppDbContext db) : IProductService
{
    private readonly AppDbContext _db = db; 
    public async Task<IEnumerable<ProductResponse>> GetAllProducts(CancellationToken cancellationToken = default)
    {
        var products = await _db.Products.ToListAsync(cancellationToken);
        return products.Adapt<IEnumerable<ProductResponse>>();
    }
    public async Task<Result<ProductResponse>> GetProductById(int productId, CancellationToken cancellationToken = default)
    {
        var product = await _db.Products.FirstOrDefaultAsync(p => p.ProductId == productId, cancellationToken);
        if (product == null)
            return Result.Failure<ProductResponse>(ProductErrors.productNotFound);
        return Result.Success(product.Adapt<ProductResponse>());
    }
    public async Task<Result<Product>> CreateProduct(ProductRequest productRequest, CancellationToken cancellationToken = default)
    {

        var validationResult = FluentValidationHelper.Validate(new ProductRequestValidator(), productRequest);
        if (!validationResult.IsValid)
            return Result.Failure<Product>(Error.FromValidationResult(validationResult));

        var product = productRequest.Adapt<Product>();

        await _db.Products.AddAsync(product);
        await _db.SaveChangesAsync(cancellationToken);
        return Result.Success(product);

    }
    public async Task<Result<ProductResponse>> UpdateProduct(int productId, ProductRequest productRequest, CancellationToken cancellationToken = default)
    {
       var validationResult = FluentValidationHelper.Validate(new ProductRequestValidator(), productRequest);
        if (!validationResult.IsValid)
            return Result.Failure<ProductResponse>(Error.FromValidationResult(validationResult));

        var product = await _db.Products.FirstOrDefaultAsync(p => p.ProductId == productId, cancellationToken);
        if (product == null)
            return Result.Failure<ProductResponse>(ProductErrors.productNotFound);

        product = productRequest.Adapt(product);

        await _db.SaveChangesAsync();
        return Result.Success(product.Adapt<ProductResponse>());
    }

    public async Task<Result<bool>> DeleteProduct(int productId, CancellationToken cancellationToken = default)
    {
        var product = await _db.Products.FirstOrDefaultAsync(p => p.ProductId == productId, cancellationToken);
        if (product == null)
            return Result.Failure<bool>(ProductErrors.productNotFound);
        _db.Products.Remove(product);
        await _db.SaveChangesAsync(cancellationToken);
        return Result.Success(true);
    }



}
