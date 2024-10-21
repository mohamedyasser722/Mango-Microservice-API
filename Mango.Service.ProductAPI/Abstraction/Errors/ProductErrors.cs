using Mango.Services.ProductAPI.Abstraction.ResultErrorHandellingPattern;

namespace Mango.Service.ProductAPI.Abstraction.Errors;

public static class ProductErrors
{
    public static readonly Error productNotFound = new Error("ProductNotFound", "Product not found", StatusCodes.Status404NotFound);
}
