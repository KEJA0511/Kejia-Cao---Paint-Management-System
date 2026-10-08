namespace PaintSystem; 

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
