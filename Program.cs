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
        PaintStore store = new PaintStore(products, new List<int>{ 20, 15, 8 });

        Console.WriteLine("=== Available products ===");
        for (int i = 0; i < store.Products.Count; i++)
        {
            store.Products[i].DisplayInfo();
            Console.WriteLine($"Stock: {store.Quantities[i]}");
            Console.WriteLine();
        }

        Console.WriteLine($"Default price of {product1.Name}: {product1.GetFinalPrice():F2}");
        IBuyable buyable = product1;
        Console.WriteLine($"Price with a 10% discount: {buyable.GetFinalPrice(0.10m):F2}");

        Order order = new Order(new List<PaintProduct>{ product1 }, new List<int>{ 2 });
        Order multipleOrder = new Order(products, new List<int>{ 2, 3, 1 });
        Order storeOrder = new Order(
            new List<PaintProduct>{ store.Products[0], store.Products[2] },
            new List<int>{ 2, 1 });

        Console.WriteLine();
        Console.WriteLine("=== Order details ===");
        order.DisplayOrder();
        multipleOrder.DisplayOrder();
        storeOrder.DisplayOrder();

        User user = new User(new List<Order>{ order, multipleOrder, storeOrder }, "Keja");
        Payment payment1 = new Payment(order, 1, PaymentStatus.Success, order.TotalPrice, PaymentMethod.Alipay, user);
        Payment payment2 = new Payment(multipleOrder, 2, PaymentStatus.Pending, multipleOrder.TotalPrice, PaymentMethod.CreditCard, user);
        Payment payment3 = new Payment(storeOrder, 3, PaymentStatus.Success, storeOrder.TotalPrice, PaymentMethod.BankTransfer, user);
        user.HistoryPayments.Add(payment1);
        user.HistoryPayments.Add(payment2);
        user.HistoryPayments.Add(payment3);

        Console.WriteLine();
        Console.WriteLine($"=== History for {user.UserName} ===");
        Console.WriteLine($"Most expensive order: {user.GetMostExpensiveOrder()?.TotalPrice:F2}");
        Console.WriteLine($"Newest order created at: {user.GetNewestOrder()?.CreatedAt}");
        Console.WriteLine($"Lowest payment: {user.GetLowestPayment()?.PaymentAmount:F2}");
        Console.WriteLine($"Newest payment ID: {user.GetNewestPayment()?.PaymentId}");
        List<Payment>? paymentsOverTen = user.GetMoreThanTen();
        if (paymentsOverTen != null)
        {
            foreach (Payment payment in paymentsOverTen)
            {
                Console.WriteLine($"Payment {payment.PaymentId}: {payment.PaymentAmount:F2}, {payment.PaymentMethod}, {payment.PaymentStatus}");
            }
        }
    }
}
