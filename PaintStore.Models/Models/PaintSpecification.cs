using System.ComponentModel.DataAnnotations;

namespace PaintStore.Models; 

public class PaintSpecification
{
    [Required]
    public string Color {get; set; }
    [Range(1,1000000)]
    public int SizeInLiters {get; set; }

    public PaintSpecification(string color, int sizeInLiters){
        Color = color; 
        SizeInLiters = sizeInLiters; 
    }

    //输出规格信息
    public void DisplaySpecification()
    {
        Console.WriteLine($"The Color is {Color}, and the size in liters is {SizeInLiters}. "); 
    }
}