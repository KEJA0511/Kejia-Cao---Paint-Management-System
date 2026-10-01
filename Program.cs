namespace PaintSystem;

class Program
{
    static void Main(string[] args)
    {
        // 创建三种油漆产品，并显示它们的信息。
        PaintSpecification specification1 = new PaintSpecification("White", 5);
        PaintSpecification specification2 = new PaintSpecification("Blue", 2);
        PaintSpecification specification3 = new PaintSpecification("Grey", 10);

        PaintProduct product1 = new PaintProduct("White Primer", PaintType.BaseCoat, specification1, 100m);
        PaintProduct product2 = new PaintProduct("Blue Gloss", PaintType.Glossy, specification2, 80m);
        PaintProduct product3 = new PaintProduct("Grey Matte", PaintType.Matte, specification3, 120m);
        PaintProduct[] products = { product1, product2, product3 };

        Console.WriteLine("=== Available products ===");
        foreach (PaintProduct product in products)
        {
            product.DisplayInfo();
            Console.WriteLine();
        }

        // 测试 1：原价 100，默认折扣 5%，折后再加税 10%，应为 104.50。
        decimal defaultPrice = product1.GetFinalPrice();
        Console.WriteLine($"Default price: expected 104.50, actual {defaultPrice:F2} - {(defaultPrice == 104.50m ? "PASS" : "FAIL")}");

        // 测试 2：通过接口传入 10% 折扣，原价 100 的折后含税价应为 99.00。
        IBuyable buyable = product1;
        decimal customPrice = buyable.GetFinalPrice(0.10m);
        Console.WriteLine($"Custom price: expected 99.00, actual {customPrice:F2} - {(customPrice == 99m ? "PASS" : "FAIL")}");

        // 测试 3：按当前选择较大折扣的设计，传入 3% 时仍应得到默认的 5%。
        decimal discount = product1.GetMaxDiscount(0.03m, true);
        Console.WriteLine($"Selected discount: expected 5%, actual {discount:P0} - {(discount == 0.05m ? "PASS" : "FAIL")}");

        // 测试 4：购买两份同一种产品，订单总价应为 209.00。
        Order order = new Order(product1, 2);

        Console.WriteLine();
        Console.WriteLine("=== Order details ===");
        order.DisplayOrder();
        order.GetTotalPrice();
        Console.WriteLine($"Order total: expected 209.00, actual {order.TotalPrice:F2} - {(order.TotalPrice == 209m ? "PASS" : "FAIL")}");
    }
}
