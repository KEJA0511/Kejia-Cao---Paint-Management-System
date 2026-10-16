using System.ComponentModel.DataAnnotations;

namespace PaintStore.Models; 

public class Brand
{
    [Required]
    public string Name {get; set; }//品牌名

    public Brand(string name)
    {
        Name = name; 
    }
}