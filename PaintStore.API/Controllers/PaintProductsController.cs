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

    [HttpGet("all")]
    public ActionResult<IEnumerable<PaintProduct>> GetAllMock()
    {
        return ProductsMock.OrderBy(product => product.ProductId).ToList(); 
    }

    

}
