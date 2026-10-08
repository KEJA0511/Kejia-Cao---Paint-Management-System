namespace PaintSystem.Tests;

public class PaintStoreTests
{
    [Fact]
    public void Constructor_PreservesProductAndStockMapping()
    {
        List<PaintProduct> products = TestData.CreateProducts();
        List<int> stock = new List<int>{ 20, 15, 8 };
        PaintStore store = new PaintStore(products, stock);

        Assert.Same(products, store.Products);
        Assert.Same(stock, store.Quantities);
        Assert.Equal(new int[]{ 10, 20, 30 }, store.Products.Select(p => p.ProductId));
        Assert.Equal(new int[]{ 20, 15, 8 }, store.Quantities);
    }

    [Fact]
    public void OrderFromStore_UsesSelectedProductsAndPurchaseQuantities()
    {
        PaintStore store = new PaintStore(TestData.CreateProducts(), new List<int>{ 20, 15, 8 });
        Order order = new Order(
            new List<PaintProduct>{ store.Products[0], store.Products[2] },
            new List<int>{ 2, 1 });

        Assert.Equal(334.40m, order.TotalPrice);
        Assert.Equal(new int[]{ 20, 15, 8 }, store.Quantities);
    }

    [Fact]
    public void Constructor_RejectsMismatchedProductAndStockCounts()
    {
        Assert.Throws<ArgumentException>(() =>
            new PaintStore(TestData.CreateProducts(), new List<int>{ 20, 15 }));
    }
}
