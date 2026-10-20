using System.Runtime.InteropServices.Swift;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaintStore.Models; 
public class PaintProduct: IBuyable
{
    public readonly decimal TaxRate = 0.10m; 
    public const decimal DefaultDiscount = 0.05m;

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ProductId {get;private set; } //这里没有private set会报错

    [Required]
    [MaxLength(100)]
    public string Name {get; set;}
    [Required]
    public Brand Brand {get; set;}
    [EnumDataType(typeof(PaintType))]
    public PaintType Type {get; set;}
    [Required]
    public PaintSpecification Specification {get; set;}
    [Range(0,100000000000)]
    public decimal Price {get; set;}
    [Required]
    public DateTime CreatedDate {get;set;} = DateTime.Now; 
    
    private PaintProduct(){}

    public PaintProduct(int productId, string name, Brand brand, PaintType type, PaintSpecification specification, decimal price)
    {
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