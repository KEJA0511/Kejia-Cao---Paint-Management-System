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
            quantity: new List<int> {1})
    };

    [HttpGet("All")]
    public ActionResult<IEnumerable<Order>> GetAllOrders([FromQuery]int pageNumber, [FromQuery]int pageSize)
    {
        return OrdersMock.OrderBy(order => order.CreatedAt).Skip((pageNumber-1)*pageSize).Take(pageSize).ToList(); 
    }

    [HttpPost("createOrder")]
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

    [HttpGet("GetPaginatedOrders")]
    public IActionResult GetPaginatedOrders([FromQuery]int pageNumber, [FromQuery]int pageSize)
    {
        if( pageNumber <= 0 || pageSize <=0 )
        {
            return BadRequest("参数不合法"); 
        }
        var orders = OrdersMock.OrderBy(order => order.OrderId).Skip((pageNumber-1)*pageSize).Take(pageSize).ToList(); 
        return Ok(orders); 
    }

    [HttpGet("GetOrdersByPriceRange")]
    public IActionResult GetOrdersByPriceRange([FromQuery]int min, [FromQuery]int max)
    {
        if(min>max) return BadRequest("Invalid Value! "); 
        var orders = OrdersMock.Where(order => order.TotalPrice >= min
                                            && order.TotalPrice <= max).ToList(); 
        return Ok(orders); 
    }

    [HttpGet("GetOrdersByPaintId")]
    public IActionResult GetOrdersByPaintId([FromQuery]int paintId)
    {
        var order = OrdersMock.Where(o => o.Products.Any(p => p.ProductId == paintId)).ToList(); 
        return Ok(order); 
    }

    [HttpGet("GetOrdersByUserId")]
    public IActionResult GetOrdersByUserId([FromQuery]int userId)
    {
        var order = OrdersMock.Where(o => o.UserId == userId).ToList(); 
        return Ok(order); 
    }

    [HttpGet("GetLastMonthOrders")]
    public IActionResult GetLastMonthOrders()
    {
        var now = DateTime.Now; 
        var startOfThisMonth = new DateTime(now.Year, now.Month, 1);
        var startOfLastMonth = startOfThisMonth.AddMonths(-1);

        var order = OrdersMock.Where(o => o.CreatedAt>=startOfLastMonth&&
                                        o.CreatedAt<startOfThisMonth); 
        return Ok(order); 
    }

    [HttpGet("GetOrdersByDate")]
    public IActionResult GetOrdersByDate([FromQuery]DateTime date)
    {
        var order = OrdersMock.Where(o => o.CreatedAt.Date == date.Date); 
        return Ok(order); 
    }
}
