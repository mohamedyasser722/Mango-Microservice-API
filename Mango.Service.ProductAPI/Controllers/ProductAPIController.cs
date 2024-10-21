using Mango.Service.ProductAPI.Contracts.Product;
using Mango.Service.ProductAPI.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Service.ProductAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ProductAPIController(IProductService productService) : ControllerBase
{
    private readonly IProductService _productService = productService;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _productService.GetAllProducts();
        return Ok(result);
    }

    [HttpGet("{Id}")]
    public async Task<IActionResult> Get(int Id)
    {
        var result = await _productService.GetProductById(Id);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductRequest productRequest)
    {
        var result = await _productService.CreateProduct(productRequest);
      
        return result.IsSuccess ? CreatedAtAction(nameof(Get), new { Id = result.Value.ProductId }, result.Value) : result.ToProblem();
    }

    [HttpPut("{Id}")]
    public async Task<IActionResult> Update(int Id, [FromBody] ProductRequest productRequest)
    {
        var result = await _productService.UpdateProduct(Id, productRequest);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpDelete("{Id}")]
    public async Task<IActionResult> Delete(int Id)
    {
        var result = await _productService.DeleteProduct(Id);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}
