namespace PaintSystem.Tests;

public class PaintProductTests
{
    [Fact]
    public void Constructor_SavesProductIdAndSuppliedBrand()
    {
        Brand brand = new Brand("Example Paint");
        PaintProduct product = new PaintProduct(10, "White Primer", brand,
            PaintType.BaseCoat, new PaintSpecification("White", 5), 100m);

        Assert.Equal(10, product.ProductId);
        Assert.Same(brand, product.Brand);
    }

    [Fact]
    public void GetFinalPrice_AppliesDefaultDiscountThenTax()
    {
        PaintProduct product = TestData.CreateProduct(10, 100m, PaintType.BaseCoat);

        Assert.Equal(104.50m, product.GetFinalPrice());
    }

    [Fact]
    public void GetFinalPrice_ThroughInterface_AppliesRequestedLargerDiscount()
    {
        IBuyable product = TestData.CreateProduct(10, 100m, PaintType.BaseCoat);

        Assert.Equal(99m, product.GetFinalPrice(0.10m));
    }

    [Fact]
    public void GetMaxDiscount_KeepsDefaultWhenRequestedDiscountIsSmaller()
    {
        PaintProduct product = TestData.CreateProduct(10, 100m, PaintType.BaseCoat);

        Assert.Equal(0.05m, product.GetMaxDiscount(0.03m, true));
        Assert.Equal(104.50m, product.GetFinalPrice(0.03m));
    }
}
