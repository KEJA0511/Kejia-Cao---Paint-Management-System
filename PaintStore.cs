namespace PaintSystem; 

class PaintStore
{
    public PaintProduct[] Products {get;set;}
    public int[] Quantities {get;set;}

    public PaintStore(PaintProduct[] products, int[] quantities)
    {
        if(products.Length != quantities.Length) //数组长度检查
        {
            throw new ArgumentException("The size of products and quantities array must be the same. "); 
        }
        Products = products; 
        Quantities = quantities; 
    }


}