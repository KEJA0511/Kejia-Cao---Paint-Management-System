using System.IO.Pipes;

namespace PaintSystem; 

class Payment
{
    public Order Order{get;set;}
    public int PaymentId{get;set;}
    public PaymentStatus PaymentStatus{get;set;}
    public decimal PaymentAmount{get;set;}
    public PaymentMethod PaymentMethod{get;set;}
    public User User{get;set;}
    public DateTime CreatedAt {get;}

    public Payment(Order order, int paymentId, PaymentStatus paymentStatus, decimal paymentAmount, PaymentMethod paymentMethod, User user)
    {
        Order = order; 
        PaymentId = paymentId; 
        PaymentStatus = paymentStatus; 
        PaymentAmount = paymentAmount; 
        PaymentMethod = paymentMethod; 
        CreatedAt = DateTime.Now; 
        User = user; 
    }
}
