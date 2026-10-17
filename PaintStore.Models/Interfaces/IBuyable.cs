namespace PaintStore.Models; 

public interface IBuyable
{
    decimal GetFinalPrice(decimal discount);
}