namespace PaintSystem.Tests;

public class OrderTests
{
    [Fact]
    public void Constructor_CalculatesSingleProductTotalIncludingQuantity()
    {
        PaintProduct product = TestData.CreateProduct(10, 100m, PaintType.BaseCoat);
        Order order = new Order(new List<PaintProduct>{ product }, new List<int>{ 2 });

        Assert.Equal(209m, order.TotalPrice);
    }

    [Fact]
    public void Constructor_CalculatesMultipleProductTotal()
    {
        Order order = TestData.CreateOrder();

        Assert.Equal(585.20m, order.TotalPrice);
    }

    [Fact]
    public void GetTotalOrderPrice_RepeatedCallsDoNotAccumulate()
    {
        Order order = TestData.CreateOrder();

        Assert.Equal(585.20m, order.GetTotalOrderPrice());
        Assert.Equal(585.20m, order.GetTotalOrderPrice());
        Assert.Equal(585.20m, order.TotalPrice);
    }

    [Fact]
    public void Constructor_RejectsMismatchedProductAndQuantityCounts()
    {
        ArgumentException exception = Assert.Throws<ArgumentException>(() =>
            new Order(TestData.CreateProducts(), new List<int>{ 2, 3 }));

        Assert.Equal("quantity", exception.ParamName);
    }

    [Fact]
    public void GetMostExpensivePaintProduct_ReturnsProductWithHighestOriginalPrice()
    {
        Order order = TestData.CreateOrder();

        Assert.Same(order.Products[2], order.GetMostExpensivePaintProduct());
    }

    [Fact]
    public void GetMostExpensivePaintProduct_WhenPricesTie_ReturnsFirstMatchingProduct()
    {
        Order order = TestData.CreateOrder();
        order.Products[0].Price = 120m;

        Assert.Same(order.Products[0], order.GetMostExpensivePaintProduct());
    }

    [Fact]
    public void RemoveProduct_RemovesCorrespondingQuantityAndUpdatesTotal()
    {
        Order order = TestData.CreateOrder();

        order.RemoveProduct(20);

        Assert.Equal(new int[]{ 10, 30 }, order.Products.Select(p => p.ProductId));
        Assert.Equal(new int[]{ 2, 1 }, order.Quantity);
        Assert.Equal(334.40m, order.TotalPrice);
    }

    [Fact]
    public void RemoveProduct_UnknownId_LeavesOrderUnchanged()
    {
        Order order = TestData.CreateOrder();

        order.RemoveProduct(999);

        Assert.Equal(new int[]{ 10, 20, 30 }, order.Products.Select(p => p.ProductId));
        Assert.Equal(new int[]{ 2, 3, 1 }, order.Quantity);
        Assert.Equal(585.20m, order.TotalPrice);
    }

    [Fact]
    public void RemoveProduct_LastRemainingProduct_LeavesEmptyOrderAndZeroTotal()
    {
        PaintProduct product = TestData.CreateProduct(10, 100m, PaintType.BaseCoat);
        Order order = new Order(new List<PaintProduct>{ product }, new List<int>{ 2 });

        order.RemoveProduct(10);

        Assert.Empty(order.Products);
        Assert.Empty(order.Quantity);
        Assert.Equal(0m, order.TotalPrice);
    }

    [Fact]
    public void GetAllPaintBetween_UsesStrictBoundsAndLeavesSourceUnchanged()
    {
        Order order = TestData.CreateOrder();

        List<PaintProduct> result = order.GetAllPaintBetween(80m, 120m);

        Assert.Same(order.Products[0], Assert.Single(result));
        Assert.Equal(3, order.Products.Count);
    }

    [Fact]
    public void GetTotalPriceByType_GroupsFinalPricesIncludingQuantities()
    {
        Order order = TestData.CreateOrder();
        order.Products[2].Type = PaintType.BaseCoat;

        Dictionary<PaintType, decimal> result = order.GetTotalPriceByType();

        Assert.Equal(2, result.Count);
        Assert.Equal(334.40m, result[PaintType.BaseCoat]);
        Assert.Equal(250.80m, result[PaintType.Glossy]);
        Assert.Equal(order.TotalPrice, result.Values.Sum());
    }

    [Fact]
    public void EmptyOrder_QueriesAndRemovalReturnEmptyResults()
    {
        Order order = new Order(new List<PaintProduct>(), new List<int>());

        order.RemoveProduct(10);

        Assert.Null(order.GetMostExpensivePaintProduct());
        Assert.Empty(order.GetAllPaintBetween(0m, 100m));
        Assert.Empty(order.GetTotalPriceByType());
        Assert.Equal(0m, order.TotalPrice);
    }
}
