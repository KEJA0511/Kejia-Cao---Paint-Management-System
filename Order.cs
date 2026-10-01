namespace PaintSystem; 

class Order
{
    public DateTime CreatedAt{get; }//订单创建时间
    public PaintProduct Product{get; set; } 
    public int Quantity {get; set;}
    public decimal TotalPrice{get;set;}

    public Order(PaintProduct product, int quantity)
    {
        CreatedAt = DateTime.Now; 
        Quantity = quantity; 
        Product = product; 
        TotalPrice = Product.GetFinalPrice() * Quantity; 
    }

    public void DisplayOrder()
    {
        Console.WriteLine($"The order is created at {CreatedAt}. ");
        Console.WriteLine($"The product information are:");
        Product.DisplayInfo();
        Console.WriteLine($"The quantity is {Quantity}"); 
        Console.WriteLine($"The total price is {TotalPrice}"); 
    }

    public void GetTotalPrice()
    {
        //输出订单中产品的折后含税单价
        Console.WriteLine($"The price of {Product.Name} is {Product.GetFinalPrice()}");
    }
}