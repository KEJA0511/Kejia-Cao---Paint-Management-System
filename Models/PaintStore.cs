namespace PaintSystem; 

class PaintStore
{
    public List<PaintProduct> Products {get;set;}
    public List<int> Quantities {get;set;}

    public PaintStore(List<PaintProduct> products, List<int> quantities)
    {
        if(products.Count != quantities.Count) //数组长度检查
        {
            throw new ArgumentException("The size of products and quantities array must be the same. "); 
        }
        Products = products; 
        Quantities = quantities; 
    }

}