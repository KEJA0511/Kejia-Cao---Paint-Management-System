namespace PaintSystem; 

class Order
{
    public DateTime CreatedAt{get; }//订单创建时间
    public List<PaintProduct> Products{get; set; } 
    public List<int> Quantity {get; set;}
    public decimal TotalPrice {get;set;}

    public Order(List<PaintProduct> product, List<int> quantity)
    {
        if (product.Count != quantity.Count)
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
        for (int i = 0; i < Products.Count; i++)
        {
            total += Products[i].GetFinalPrice() * Quantity[i];
        }
        return total;
    }

    public void DisplayOrder()
    {
        Console.WriteLine($"The order is created at {CreatedAt}. ");
        Console.WriteLine($"The product information are:");
        for (int i = 0; i < Products.Count; i++)
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