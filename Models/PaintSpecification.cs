namespace PaintSystem; 

class PaintSpecification
{
    public string Color {get; set; }
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