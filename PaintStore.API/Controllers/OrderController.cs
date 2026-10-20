using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaintStore.Models;

namespace PaintStore.API.Controllers; 

[Route("api/[controller]")]
[ApiController]
public sealed class OrderController : ControllerBase
{
    private readonly List<Order> OrdersMock = new()
    {
        new Order(
            orderId: 1, 
            products: new List<PaintProduct>
            {
                new PaintProduct(
                    productId: 1,
                    name: "Interior Matte",
                    brand: new Brand("Example Paint"),
                    type: PaintType.Matte,
                    specification: new PaintSpecification("White", 5),
                    price: 120m)
            },
            quantity: new List<int> { 2 }),
        new Order(
            orderId: 2, 
            products: new List<PaintProduct>
            {
                new PaintProduct(
                    productId: 2,
                    name: "Exterior Gloss",
                    brand: new Brand("Sample Colours"),
                    type: PaintType.Glossy,
                    specification: new PaintSpecification("Blue", 2),
                    price: 80m)
            },
            quantity: new List<int> { 3 }),
        new Order(
            orderId: 3, 
            products: new List<PaintProduct>
            {
                new PaintProduct(
                    productId: 3,
                    name: "Primer",
                    brand: new Brand("Example Paint"),
                    type: PaintType.BaseCoat,
                    specification: new PaintSpecification("Grey", 10),
                    price: 100m)
            },
            quantity: new List<int> { 1 })
    };

    [HttpGet("all")]
    public ActionResult<IEnumerable<Order>> GetAll()
    {
        return OrdersMock.OrderBy(order => order.CreatedAt).ToList(); 
    }

    [HttpPost("Orders/createOrder")]
    public IActionResult CreateOrder([FromBody]Order order)
    {
        OrdersMock.Add(order); 
        return Created($"/api/Order/{order.OrderId}", order); //返回createdResult对象，用来描述相应
    }

    [HttpGet("FilterOrders")]
    public IActionResult FilterOrders([FromQuery] decimal minPrice, [FromQuery] decimal maxPrice)
    {
        var orders = OrdersMock.Where(order => order.TotalPrice > minPrice
                                        && order.TotalPrice < maxPrice).ToList(); 
        
        return Ok(orders); 
    }

    [HttpGet("Orders/GetPaginatedOrders")]
    public IActionResult GetPaginatedOrders([FromQuery]int pageNumber, [FromQuery]int pageSize)
    {
        if( pageNumber <= 0 || pageSize <=0 )
        {
            return BadRequest("参数不合法"); 
        }
        var orders = OrdersMock.OrderBy(order => order.OrderId).Skip((pageNumber-1)*pageSize).Take(pageSize).ToList(); 
        return Ok(orders); 
    }
}

