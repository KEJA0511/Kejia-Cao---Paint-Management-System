using System.IO.Pipes;

namespace PaintSystem; 
enum PaymentStatus
{
    Pending, 
    Failed, 
    Success
}
enum PaymentMethod
{   
    Alipay, 
    CreditCard, 
    BankTransfer
}

class User
{
    public List<Order> HistoryOrders{get;set;}
    public string UserName{get;set;}
    public List<Payment> HistoryPayments{get;set;}

    public User(List<Order> historyOrders, string userName)
    {
        HistoryOrders = historyOrders; 
        UserName = userName; 
        HistoryPayments = new List<Payment>(); 
    }


    public Order? GetMostExpensiveOrder()
    {
        return HistoryOrders.OrderByDescending(order => order.TotalPrice).FirstOrDefault(); 
    }

    public Order? GetNewestOrder()
    {
        return HistoryOrders.OrderByDescending(order => order.CreatedAt).FirstOrDefault(); 
    }

    public Payment? GetLowestPayment()
    {
        return HistoryPayments.OrderBy(payment => payment.PaymentAmount).FirstOrDefault(); 
    }

    public Payment? GetNewestPayment()
    {
        return HistoryPayments.OrderByDescending(payment => payment.CreatedAt).FirstOrDefault(); 
    }

    public List<Payment>? GetMoreThanTen()
    {
        return HistoryPayments.Where(payment => payment.PaymentAmount>10m).ToList();
    }

}

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