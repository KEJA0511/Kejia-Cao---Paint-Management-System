namespace PaintSystem.Tests;

internal static class TestData
{
    internal static PaintProduct CreateProduct(int id, decimal price, PaintType type)
    {
        return new PaintProduct(id, $"Paint {id}", new Brand("Example Paint"),
            type, new PaintSpecification("White", 5), price);
    }

    internal static List<PaintProduct> CreateProducts()
    {
        return new List<PaintProduct>
        {
            CreateProduct(10, 100m, PaintType.BaseCoat),
            CreateProduct(20, 80m, PaintType.Glossy),
            CreateProduct(30, 120m, PaintType.Matte)
        };
    }

    internal static Order CreateOrder()
    {
        return new Order(CreateProducts(), new List<int>{ 2, 3, 1 });
    }
}
