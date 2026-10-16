using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PaintStore.Models; 

public class Order
{
    [Required]
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int OrderId{get; }
    [Required]
    public DateTime CreatedAt{get; } = DateTime.Now; //订单创建时间
    [Required]
    public List<PaintProduct> Products{get; set; }
    [Required]
    public List<int> Quantity {get; set;}
    [Required]
    [Range(0,10000000)]
    public decimal TotalPrice {get;set;}
    [Required]
    public int UserId{get;}

    private Order (){}
    public Order(int orderId, List<PaintProduct> products, List<int> quantity)
    {
        foreach (int quantityItem in quantity)
        {
            if(quantityItem < 1 || quantityItem > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(quantity), "The quantity should be greater than zero. "); 
            }
        }

        if (products.Count != quantity.Count)
        {
            throw new ArgumentException("Each product must have a corresponding quantity.", nameof(quantity));
        }

        OrderId = orderId; 
        Quantity = quantity; 
        Products = products; 
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

    public PaintProduct? GetMostExpensivePaintProduct()
    {
        //找出最贵的油漆
        return Products.OrderByDescending(p=>p.Price).FirstOrDefault(); 
    }

    public void RemoveProduct(int productId)
    {
        //根据Product ID 删除指定的油漆
        int index = Products.FindIndex(p => p.ProductId == productId); 
        if(index == -1) return; //若找不到则返回
        Products.RemoveAt(index); 
        Quantity.RemoveAt(index); 
        TotalPrice = GetTotalOrderPrice(); //更新删除商品后的总价
    }

    public List<PaintProduct> GetAllPaintBetween(decimal x, decimal y)
    {
        return Products.Where(p => p.Price>x && p.Price<y).ToList(); 
    }

    public Dictionary<PaintType, decimal> GetTotalPriceByType()
    {
        Dictionary<PaintType, decimal> totalPrice= new Dictionary<PaintType, decimal>(); 
        List<PaintType> types = Products.Select(p => p.Type).Distinct().ToList(); 
        int count = types.Count(); 

        for (int i = 0; i< count; i++)
        {
            PaintType type = types[i]; 

            decimal total = Products.Select((p, index) => {
                if(p.Type == type)
                {
                    return p.GetFinalPrice()*Quantity[index]; 
                }

                return 0m; 
            }).Sum(); 

            totalPrice.Add(type, total); 
        }

    return totalPrice; 

    }
}