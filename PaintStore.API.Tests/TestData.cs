using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using PaintStore.API.Controllers;
using PaintStore.Models;

namespace PaintStore.API.Tests;

internal static class TestData
{
    internal static PaintProduct Product(int id, decimal price) => new(
        id, $"Paint {id}", new Brand("Test Brand"), PaintType.Matte,
        new PaintSpecification("White", 5), price);

    internal static Order Order(int id, decimal total, DateTime createdAt,
        int userId = 1, params PaintProduct[] products)
    {
        var order = new Order(id, products.ToList(), products.Select(_ => 1).ToList());
        order.TotalPrice = total;
        // Test-only fixture setup: these properties have no public setters.
        // This does not validate how the production constructor assigns UserId.
        SetBackingField(order, nameof(order.CreatedAt), createdAt);
        SetBackingField(order, nameof(order.UserId), userId);
        return order;
    }

    internal static void SeedOrders(OrderController controller, params Order[] orders)
    {
        var field = typeof(OrderController).GetField("OrdersMock",
            BindingFlags.Instance | BindingFlags.NonPublic)
            ?? throw new InvalidOperationException("OrdersMock field was not found.");
        var list = Assert.IsType<List<Order>>(field.GetValue(controller));
        list.Clear();
        list.AddRange(orders);
    }

    internal static T[] ReadOk<T>(IActionResult result)
    {
        var ok = Assert.IsType<OkObjectResult>(result);
        return Assert.IsAssignableFrom<IEnumerable<T>>(ok.Value).ToArray();
    }

    private static void SetBackingField<T>(Order order, string property, T value)
    {
        var field = typeof(Order).GetField($"<{property}>k__BackingField",
            BindingFlags.Instance | BindingFlags.NonPublic)
            ?? throw new InvalidOperationException($"Backing field for {property} was not found.");
        field.SetValue(order, value);
    }
}
