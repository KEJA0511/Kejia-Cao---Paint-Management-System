using Microsoft.AspNetCore.Mvc;
using PaintStore.API.Controllers;
using PaintStore.Models;

namespace PaintStore.API.Tests;

public class PaintProductsControllerTests
{
    [Theory]
    [InlineData(1, 2, new int[] { 1, 2 })]
    [InlineData(2, 2, new int[] { 3 })]
    [InlineData(3, 2, new int[] { })]
    public void GetAllPaintProducts_ReturnsRequestedPage(int page, int size, int[] expectedIds)
    {
        var controller = new PaintProductsController();

        var result = controller.GetAllPaintProducts(pageSize: size, pageNumber: page);

        Assert.Equal(expectedIds, result.Value!.Select(p => p.ProductId));
    }

    // Product price filtering currently uses an inclusive min and exclusive max.
    [Theory]
    [InlineData(80, 120, 1, 10, new int[] { 2, 3 })]
    [InlineData(80, 121, 1, 1, new int[] { 2 })]
    [InlineData(80, 121, 2, 1, new int[] { 3 })]
    [InlineData(80, 121, 4, 1, new int[] { })]
    [InlineData(80, 80, 1, 10, new int[] { })]
    [InlineData(200, 300, 1, 10, new int[] { })]
    public void GetProductsByPriceRange_FiltersSortsAndThenPages(
        int min, int max, int page, int size, int[] expectedIds)
    {
        var controller = new PaintProductsController();

        var result = controller.GetProductsByPriceRange(min, max, pageSize: size, pageNumber: page);

        Assert.Equal(expectedIds, TestData.ReadOk<PaintProduct>(result).Select(p => p.ProductId));
    }

    [Fact]
    public void GetProductsByPriceRange_SupportsFractionalPrices()
    {
        var controller = new PaintProductsController();

        var result = controller.GetProductsByPriceRange(99.99m, 100.01m, 10, 1);

        Assert.Equal(new[] { 3 }, TestData.ReadOk<PaintProduct>(result).Select(p => p.ProductId));
    }

    [Theory]
    [InlineData(2, new int[] { 2 })]
    [InlineData(999, new int[] { })]
    public void GetPaintProductByPaintId_ReturnsOnlyMatchingProducts(int id, int[] expectedIds)
    {
        var controller = new PaintProductsController();

        var result = controller.GetPaintProductByPaintId(id);

        Assert.Equal(expectedIds, TestData.ReadOk<PaintProduct>(result).Select(p => p.ProductId));
    }

    [Fact]
    public void GetPaintProductByUserId_WithNoMatchingUser_ReturnsAnEmptyProductCollection()
    {
        var controller = new PaintProductsController();

        var result = controller.GetPaintProductByUserId(999);

        // A bodyless Ok() is not a product query result.
        Assert.Empty(TestData.ReadOk<PaintProduct>(result));
    }
}
