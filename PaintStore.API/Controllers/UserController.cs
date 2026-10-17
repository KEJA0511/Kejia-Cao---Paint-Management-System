using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaintStore.Models;

namespace PaintStore.API.Controllers; 

[Route("api/[controller]")]
[ApiController]
public sealed class UserController : ControllerBase
{
    private readonly List<User> UsersMock = new()
    {
        new User(
            userId: 1,
            historyOrders: new List<Order>(),
            userName: "Alice",
            email: "alice@example.com",
            phone: "0400000001"),
        new User(
            userId: 2,
            historyOrders: new List<Order>(),
            userName: "Bob",
            email: "bob@example.com",
            phone: "0400000002"),
        new User(
            userId: 3,
            historyOrders: new List<Order>(),
            userName: "Charlie",
            email: "charlie@example.com",
            phone: "0400000003")
    };

    [HttpGet("All")]
    public ActionResult<List<User>> GetUsers([FromQuery]int pageNumber, [FromQuery]int pageSize)
    {
        //获取所有用户
        //分页
        var users = UsersMock.OrderBy(u => u.UserId).Skip((pageNumber - 1)*pageSize).Take(pageSize).ToList(); 
        return Ok(users); 
    }

    [HttpGet("GetUserById")]
    public ActionResult GetUserById([FromQuery]int userId)
    {
        //通过参数userId获取指定user信息
        var user = UsersMock.Where(u => u.UserId == userId); 
        return Ok(user); 
    }

    [HttpGet("GetUserByName")]
    public ActionResult GetUserByName([FromQuery]string userName)
    {
        //通过用户名获取用户
        //用户名可能重复，所以此处ToList
        var user = UsersMock.Where(u=>u.UserName == userName).ToList(); 
        return Ok(user); 
    }

    [HttpGet("GetUserByEmail")]
    public ActionResult GetUserByEmail([FromQuery]string email)
    {
        //通过邮箱获取用户
        //邮箱是唯一值
        var user = UsersMock.Where(u => u.Email == email); 
        return Ok(user); 
    }
}

