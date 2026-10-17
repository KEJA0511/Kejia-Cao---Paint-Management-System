using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaintStore.Models;

namespace PaintStore.API.Controllers; 

[Route("api/[controller]")]
[ApiController]
public sealed class PaintProductsController : ControllerBase
{
    private readonly List<PaintProduct> ProductsMock = new()
    {
        new PaintProduct(
            productId: 1,
            name: "Interior Matte",
            brand: new Brand("Example Paint"),
            type: PaintType.Matte,
            specification: new PaintSpecification("White", 5),
            price: 120m),
        new PaintProduct(
            productId: 2,
            name: "Exterior Gloss",
            brand: new Brand("Sample Colours"),
            type: PaintType.Glossy,
            specification: new PaintSpecification("Blue", 2),
            price: 80m),
        new PaintProduct(
            productId: 3,
            name: "Primer",
            brand: new Brand("Example Paint"),
            type: PaintType.BaseCoat,
            specification: new PaintSpecification("Grey", 10),
            price: 100m)
    };

    [HttpGet("GetAllPaintProducts")]
    public ActionResult<IEnumerable<PaintProduct>> GetAllPaintProducts([FromQuery]int pageSize, [FromQuery]int pageNumber)
    {
        //分页
        return ProductsMock.OrderBy(product => product.ProductId).Skip((pageNumber-1)*pageSize).Take(pageSize).ToList(); 
    }

    [HttpGet("GetProductsByPriceRange")]
    public IActionResult GetProductsByPriceRange([FromQuery]decimal min, [FromQuery]decimal max, [FromQuery]int pageSize, [FromQuery]int pageNumber)
    {
        //分页，价格升序
        var products = ProductsMock.Where(p => p.Price>=min && p.Price<max).OrderBy(p=>p.Price).Skip((pageNumber-1)*pageSize).Take(pageSize).ToList(); 
        return Ok(products); 
    }
    
    [HttpGet("GetPaintProductsByPaintId")]
    public IActionResult GetPaintProductByPaintId([FromQuery]int paintId)
    {
        return Ok(ProductsMock.Where(p=>p.ProductId == paintId)); 
    }

    [HttpGet("GetPaintProductsByUserId")]
    public IActionResult GetPaintProductByUserId([FromQuery]int paintId)
    {
        //这里没有很懂，为什么要通过UserId查找商品呢？是查找客户买过什么商品吗？
        return Ok(); 
    }
    

}
