namespace PaintSystem; 

class Order
{
    public DateTime CreatedAt{get; }//订单创建时间
    public PaintProduct[] Products{get; set; } 
    public int[] Quantity {get; set;}
    public decimal TotalPrice {get;set;}

    public Order(PaintProduct[] product, int[] quantity)
    {
        if (product.Length != quantity.Length)
        {
            throw new ArgumentException("Each product must have a corresponding quantity.", nameof(quantity));
        }

        CreatedAt = DateTime.Now; 
        Quantity = quantity; 
        Products = product; 
        TotalPrice = GetTotalOrderPrice();
    }

    public decimal GetTotalOrderPrice()
    {
        decimal total = 0m;
        for (int i = 0; i < Products.Length; i++)
        {
            total += Products[i].GetFinalPrice() * Quantity[i];
        }
        return total;
    }

    public void DisplayOrder()
    {
        Console.WriteLine($"The order is created at {CreatedAt}. ");
        Console.WriteLine($"The product information are:");
        for (int i = 0; i < Products.Length; i++)
        {
            Products[i].DisplayInfo();
            Console.WriteLine($"The quantity is {Quantity[i]}");
        }
        Console.WriteLine($"The total price is {TotalPrice}"); 
    }

    public void GetTotalPrice()
    {
        //输出订单中产品的折后含税单价
        foreach(PaintProduct p in Products)
        Console.WriteLine($"The price of {p.Name} is {p.GetFinalPrice()}");
    }
}