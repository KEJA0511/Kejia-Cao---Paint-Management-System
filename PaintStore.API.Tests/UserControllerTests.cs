using System.Reflection;
using PaintStore.API.Controllers;
using PaintStore.Models;

namespace PaintStore.API.Tests;

public class UserControllerTests
{
    [Theory]
    [InlineData(1, 2, new int[] { 1, 2 })]
    [InlineData(2, 2, new int[] { 3 })]
    [InlineData(3, 2, new int[] { })]
    public void GetUsers_ReturnsRequestedPage(int page, int size, int[] expectedIds)
    {
        var controller = new UserController();

        var result = controller.GetUsers(page, size);

        Assert.Equal(expectedIds, TestData.ReadOk<User>(result.Result!).Select(u => u.UserId));
    }

    [Theory]
    [InlineData(2, new int[] { 2 })]
    [InlineData(999, new int[] { })]
    public void GetUserById_ReturnsOnlyMatchingUsers(int userId, int[] expectedIds)
    {
        var controller = new UserController();

        var result = controller.GetUserById(userId);

        Assert.Equal(expectedIds, TestData.ReadOk<User>(result).Select(u => u.UserId));
    }

    [Theory]
    [InlineData("Alice", new int[] { 1 })]
    [InlineData("Unknown", new int[] { })]
    public void GetUserByName_ReturnsOnlyMatchingUsers(string name, int[] expectedIds)
    {
        var controller = new UserController();

        var result = controller.GetUserByName(name);

        Assert.Equal(expectedIds, TestData.ReadOk<User>(result).Select(u => u.UserId));
    }

    [Fact]
    public void GetUserByName_ReturnsAllUsersWithTheSameName()
    {
        var controller = new UserController();
        var field = typeof(UserController).GetField("UsersMock",
            BindingFlags.Instance | BindingFlags.NonPublic)!;
        var users = Assert.IsType<List<User>>(field.GetValue(controller));
        users.Add(new User(4, new List<Order>(), "Alice", "alice2@example.com", "0400000004"));

        var result = controller.GetUserByName("Alice");

        Assert.Equal(new[] { 1, 4 }, TestData.ReadOk<User>(result).Select(u => u.UserId));
    }

    [Theory]
    [InlineData("bob@example.com", new int[] { 2 })]
    [InlineData("unknown@example.com", new int[] { })]
    public void GetUserByEmail_ReturnsOnlyMatchingUsers(string email, int[] expectedIds)
    {
        var controller = new UserController();

        var result = controller.GetUserByEmail(email);

        Assert.Equal(expectedIds, TestData.ReadOk<User>(result).Select(u => u.UserId));
    }
}
