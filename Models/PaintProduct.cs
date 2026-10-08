using System.Runtime.InteropServices.Swift;

namespace PaintSystem; 
class PaintProduct: IBuyable
{
    public readonly decimal TaxRate; 
    public const decimal DefaultDiscount = 0.05m;

    public int ProductId {get;}

    public string Name {get; set;}
    public Brand Brand {get; set;}
    public PaintType Type {get; set;}
    public PaintSpecification Specification {get; set; }
    public decimal Price {get; set; }
    
    public PaintProduct(int productId, string name, Brand brand, PaintType type, PaintSpecification specification, decimal price)
    {
        TaxRate = 0.10m; 
        ProductId = productId; 
        Brand = brand; 
        Name = name; 
        Type = type; 
        Specification = specification; 
        Price = price; 
    }

    public decimal GetFinalPrice(decimal discount = DefaultDiscount)
    {
        //计算折扣后的含税价格
        decimal finalDiscount = GetMaxDiscount(discount, true); 
        decimal finalPrice = Price * (1 - finalDiscount) * (1 + TaxRate); 
        return finalPrice; 
    }

    public void DisplayInfo()
    {
        Console.WriteLine($"The name is {Name}, the type is {Type}, the brand is {Brand.Name}. "); 
        Specification.DisplaySpecification();
        Console.WriteLine($"The original price is {Price}. ");  
    }

    public decimal GetMaxDiscount(decimal discount, bool isOverridable)
    {
        //计算折扣
        if(!isOverridable) throw new InvalidCastException("Is not allowed to edit the rate! ");
        if(discount > DefaultDiscount) return discount; 
        return DefaultDiscount; 
    }
}