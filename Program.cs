namespace PaintSystem;

class Program
{
    static void Main(string[] args)
    {
        PaintSpecification specification1 = new PaintSpecification("White", 5);
        PaintSpecification specification2 = new PaintSpecification("Blue", 2);
        PaintSpecification specification3 = new PaintSpecification("Grey", 10);

        Brand brand1 = new Brand("Example Paint");
        Brand brand2 = new Brand("Sample Colours");

        PaintProduct product1 = new PaintProduct(1, "White Primer", brand1, PaintType.BaseCoat, specification1, 100m);
        PaintProduct product2 = new PaintProduct(2, "Blue Gloss", brand1, PaintType.Glossy, specification2, 80m);
        PaintProduct product3 = new PaintProduct(3, "Grey Matte", brand2, PaintType.Matte, specification3, 120m);
        List<PaintProduct> products = new List<PaintProduct>{ product1, product2, product3 };
        List<int> stockQuantities = new List<int>{20, 15, 8}; 
        PaintStore store = new PaintStore(products, stockQuantities); 

        Console.WriteLine("=== Available products ===");
        foreach (PaintProduct product in products)
        {
            product.DisplayInfo();
            Console.WriteLine($"The brand is {product.Brand.Name}.");
            Console.WriteLine();
        }

        // 检查构造函数是否保存了各产品传入的品牌对象。
        bool brandsMatch = product1.Brand == brand1 && product2.Brand == brand1 && product3.Brand == brand2;
        Console.WriteLine($"Product brand assignments: {(brandsMatch ? "PASS" : "FAIL")}");

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
        List<PaintProduct> orderedProducts = new List<PaintProduct>{ product1 };
        List<int> quantities = new List<int>{ 2 };
        Order order = new Order(orderedProducts, quantities);

        Console.WriteLine();
        Console.WriteLine("=== Order details ===");
        order.DisplayOrder();
        order.GetTotalPrice();
        Console.WriteLine($"Order total: expected 209.00, actual {order.TotalPrice:F2} - {(order.TotalPrice == 209m ? "PASS" : "FAIL")}");

        // 测试 5：三种产品分别购买 2、3、1 件，总价为 104.50*2 + 83.60*3 + 125.40。
        List<int> multipleQuantities = new List<int>{ 2, 3, 1 };
        Order multipleOrder = new Order(products, multipleQuantities);
        Console.WriteLine();
        Console.WriteLine("=== Multiple-product order ===");
        multipleOrder.DisplayOrder();
        Console.WriteLine($"Multiple-product total: expected 585.20, actual {multipleOrder.TotalPrice:F2} - {(multipleOrder.TotalPrice == 585.20m ? "PASS" : "FAIL")}");

        // 测试 6：重复计算不会累加到旧的总价上。
        decimal firstTotal = multipleOrder.GetTotalOrderPrice();
        decimal secondTotal = multipleOrder.GetTotalOrderPrice();
        bool totalsMatch = firstTotal == 585.20m && secondTotal == 585.20m && multipleOrder.TotalPrice == 585.20m;
        Console.WriteLine($"Repeated total calculation: {(totalsMatch ? "PASS" : "FAIL")}");

        // 测试 7：产品有三种、数量只有两项时，应明确拒绝创建订单。
        bool mismatchedLengthsRejected = false;
        try
        {
            List<int> incompleteQuantities = new List<int>{ 2, 3 };
            Order invalidOrder = new Order(products, incompleteQuantities);
        }
        catch (ArgumentException exception) when (exception.ParamName == "quantity")
        {
            mismatchedLengthsRejected = true;
        }
        Console.WriteLine($"Mismatched array lengths rejected: {(mismatchedLengthsRejected ? "PASS" : "FAIL")}");

        // 测试 8：商店保存三种产品，库存分别为 20、15、8 件。
        Console.WriteLine();
        Console.WriteLine("=== Paint store tests ===");
        bool stockMatches = store.Products.Count == 3 && store.Quantities.Count == 3
            && store.Products[0] == product1 && store.Quantities[0] == 20
            && store.Products[1] == product2 && store.Quantities[1] == 15
            && store.Products[2] == product3 && store.Quantities[2] == 8;
        Console.WriteLine($"Store product-stock mapping: {(stockMatches ? "PASS" : "FAIL")}");
        for (int i = 0; i < store.Products.Count; i++)
        {
            Console.WriteLine($"{store.Products[i].Name}: stock {store.Quantities[i]}");
        }

        // 测试 9：从商店选择第一种和第三种产品，分别购买 2、1 件。
        List<PaintProduct> selectedProducts = new List<PaintProduct>{ store.Products[0], store.Products[2] };
        List<int> purchaseQuantities = new List<int>{ 2, 1 };
        Order storeOrder = new Order(selectedProducts, purchaseQuantities);
        storeOrder.DisplayOrder();
        Console.WriteLine($"Order from store: expected 334.40, actual {storeOrder.TotalPrice:F2} - {(storeOrder.TotalPrice == 334.40m ? "PASS" : "FAIL")}");

        // 测试 10：商店产品与库存数量数组长度不一致时，应拒绝创建。
        bool invalidStockRejected = false;
        try
        {
            List<int> incompleteStock = new List<int>{ 20, 15 };
            PaintStore invalidStore = new PaintStore(products, incompleteStock);
        }
        catch (ArgumentException)
        {
            invalidStockRejected = true;
        }
        Console.WriteLine($"Mismatched store arrays rejected: {(invalidStockRejected ? "PASS" : "FAIL")}");
    }
}
