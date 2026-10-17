using Microsoft.AspNetCore.Mvc;
using PaintStore.API.Controllers;
using PaintStore.Models;

namespace PaintStore.API.Tests;

public class OrderControllerTests
{
    [Fact]
    public void GetOrdersByPriceRange_WhenMinExceedsMax_ReturnsBadRequest()
    {
        // Arrange
        var controller = new OrderController();
        int min = 200;
        int max = 100;

        // Act
        var result = controller.GetOrdersByPriceRange(min, max);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Theory]
    [InlineData(100, 200, new int[] { 2, 3, 4 })]
    [InlineData(150, 150, new int[] { 3 })]
    [InlineData(300, 400, new int[] { })]
    public void GetOrdersByPriceRange_IncludesBoundariesAndExcludesOtherOrders(
        int min, int max, int[] expectedIds)
    {
        var controller = new OrderController();
        var date = new DateTime(2026, 1, 15);
        TestData.SeedOrders(controller,
            TestData.Order(1, 99.99m, date),
            TestData.Order(2, 100m, date),
            TestData.Order(3, 150m, date),
            TestData.Order(4, 200m, date),
            TestData.Order(5, 200.01m, date));

        var result = controller.GetOrdersByPriceRange(min, max);

        Assert.Equal(expectedIds, TestData.ReadOk<Order>(result).Select(o => o.OrderId));
    }

    [Theory]
    [InlineData(1, 2, new int[] { 1, 2 })]
    [InlineData(2, 2, new int[] { 3 })]
    [InlineData(3, 2, new int[] { })]
    public void GetAllOrders_ReturnsRequestedPageInCreationOrder(
        int pageNumber, int pageSize, int[] expectedIds)
    {
        var controller = new OrderController();
        var date = new DateTime(2026, 1, 1);
        TestData.SeedOrders(controller,
            TestData.Order(3, 100, date.AddDays(2)),
            TestData.Order(1, 100, date),
            TestData.Order(2, 100, date.AddDays(1)));

        var result = controller.GetAllOrders(pageNumber, pageSize);

        var orders = Assert.IsAssignableFrom<IEnumerable<Order>>(result.Value);
        Assert.Equal(expectedIds, orders.Select(o => o.OrderId));
    }

    [Theory]
    [InlineData(1, 2, new int[] { 1, 2 })]
    [InlineData(2, 2, new int[] { 3 })]
    [InlineData(3, 2, new int[] { })]
    public void GetPaginatedOrders_SortsBeforePaging(
        int pageNumber, int pageSize, int[] expectedIds)
    {
        var controller = new OrderController();
        var date = new DateTime(2026, 1, 1);
        TestData.SeedOrders(controller,
            TestData.Order(3, 100, date),
            TestData.Order(1, 100, date),
            TestData.Order(2, 100, date));

        var result = controller.GetPaginatedOrders(pageNumber, pageSize);

        Assert.Equal(expectedIds, TestData.ReadOk<Order>(result).Select(o => o.OrderId));
    }

    [Theory]
    [InlineData(0, 2)]
    [InlineData(-1, 2)]
    [InlineData(1, 0)]
    [InlineData(1, -1)]
    public void GetPaginatedOrders_WithNonPositiveArguments_ReturnsBadRequest(int page, int size)
    {
        var controller = new OrderController();
        Assert.IsType<BadRequestObjectResult>(controller.GetPaginatedOrders(page, size));
    }

    [Theory]
    [InlineData(2, new int[] { 1, 3 })]
    [InlineData(999, new int[] { })]
    public void GetOrdersByPaintId_FindsProductsAnywhereInEachOrder(int paintId, int[] expectedIds)
    {
        var controller = new OrderController();
        var date = new DateTime(2026, 1, 1);
        TestData.SeedOrders(controller,
            TestData.Order(1, 100, date, 1, TestData.Product(1, 50), TestData.Product(2, 50)),
            TestData.Order(2, 100, date, 1, TestData.Product(3, 100)),
            TestData.Order(3, 100, date, 2, TestData.Product(2, 100)));

        var result = controller.GetOrdersByPaintId(paintId);

        Assert.Equal(expectedIds, TestData.ReadOk<Order>(result).Select(o => o.OrderId));
    }

    [Theory]
    [InlineData(1, new int[] { 1, 3 })]
    [InlineData(2, new int[] { 2 })]
    [InlineData(999, new int[] { })]
    public void GetOrdersByUserId_ReturnsOnlyThatUsersOrders(int userId, int[] expectedIds)
    {
        var controller = new OrderController();
        var date = new DateTime(2026, 1, 1);
        TestData.SeedOrders(controller,
            TestData.Order(1, 100, date, 1),
            TestData.Order(2, 100, date, 2),
            TestData.Order(3, 100, date, 1));

        var result = controller.GetOrdersByUserId(userId);

        Assert.Equal(expectedIds, TestData.ReadOk<Order>(result).Select(o => o.OrderId));
    }

    [Fact]
    public void GetLastMonthOrders_UsesPreviousCalendarMonthWithExclusiveEnd()
    {
        var now = DateTime.Now;
        var thisMonth = new DateTime(now.Year, now.Month, 1);
        var lastMonth = thisMonth.AddMonths(-1);
        var controller = new OrderController();
        TestData.SeedOrders(controller,
            TestData.Order(1, 100, lastMonth.AddTicks(-1)),
            TestData.Order(2, 100, lastMonth),
            TestData.Order(3, 100, thisMonth.AddTicks(-1)),
            TestData.Order(4, 100, thisMonth),
            TestData.Order(5, 100, lastMonth.AddYears(-1)));

        var result = controller.GetLastMonthOrders();

        Assert.Equal(new[] { 2, 3 }, TestData.ReadOk<Order>(result).Select(o => o.OrderId));
    }

    [Theory]
    [InlineData(15, new int[] { 1, 2, 3 })]
    [InlineData(20, new int[] { })]
    public void GetOrdersByDate_MatchesTheWholeDay(int day, int[] expectedIds)
    {
        var controller = new OrderController();
        var date = new DateTime(2026, 1, 15);
        TestData.SeedOrders(controller,
            TestData.Order(1, 100, date),
            TestData.Order(2, 100, date.AddHours(12)),
            TestData.Order(3, 100, date.AddDays(1).AddTicks(-1)),
            TestData.Order(4, 100, date.AddDays(-1)),
            TestData.Order(5, 100, date.AddDays(1)));

        var result = controller.GetOrdersByDate(new DateTime(2026, 1, day));

        Assert.Equal(expectedIds, TestData.ReadOk<Order>(result).Select(o => o.OrderId));
    }

    [Fact]
    public void FilterOrders_UsesExclusivePriceBoundaries()
    {
        var controller = new OrderController();
        var date = new DateTime(2026, 1, 1);
        TestData.SeedOrders(controller,
            TestData.Order(1, 100, date),
            TestData.Order(2, 150, date),
            TestData.Order(3, 200, date));

        var result = controller.FilterOrders(100, 200);

        Assert.Equal(new[] { 2 }, TestData.ReadOk<Order>(result).Select(o => o.OrderId));
    }

    [Fact]
    public void CreateOrder_ReturnsCreatedAndAddsOrderToTheSameControllerInstance()
    {
        var controller = new OrderController();
        var order = TestData.Order(99, 150, new DateTime(2026, 1, 1));

        var result = controller.CreateOrder(order);

        var created = Assert.IsType<CreatedResult>(result);
        Assert.Equal("/api/Order/99", created.Location);
        Assert.Same(order, created.Value);
        Assert.Contains(order, controller.GetAllOrders(1, 100).Value!);
    }
}
