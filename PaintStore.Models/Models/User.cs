using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaintStore.Models; 

public class User
{   
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UserId{get;set;}

    public List<Order> HistoryOrders{get;set;}
    [Required]
    public string UserName{get;set;}
    [Required]
    [EmailAddress]
    public string Email{get;set;}
    [Required]
    public string Phone{get;set;}
    public List<Payment> HistoryPayments{get;set;}//空列表表示没有历史订单
    public DateTime CreatedDate{get;} = DateTime.Now; 
    public User(int userId, List<Order> historyOrders, string userName, string email, string phone)
    {
        UserId = userId; 
        HistoryOrders = historyOrders; 
        UserName = userName; 
        Email = email; 
        Phone = phone; 
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
