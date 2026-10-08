namespace PaintSystem.Tests;

public class UserTests
{
    [Fact]
    public void Constructor_SavesUserAndOrderHistoryAndInitializesPaymentHistory()
    {
        List<Order> history = new List<Order>{ TestData.CreateOrder() };
        User user = new User(history, "Keja");

        Assert.Equal("Keja", user.UserName);
        Assert.Same(history, user.HistoryOrders);
        Assert.Empty(user.HistoryPayments);
    }

    [Fact]
    public void GetMostExpensiveOrder_ReturnsHighestTotalFromUnsortedHistory()
    {
        Order expensive = TestData.CreateOrder();
        Order empty = new Order(new List<PaintProduct>(), new List<int>());
        User user = new User(new List<Order>{ empty, expensive }, "Keja");

        Assert.Same(expensive, user.GetMostExpensiveOrder());
    }

    [Fact]
    public void GetNewestOrder_ReturnsMostRecentFromUnsortedHistory()
    {
        Order older = TestData.CreateOrder();
        Thread.Sleep(20);
        Order newer = new Order(new List<PaintProduct>(), new List<int>());
        User user = new User(new List<Order>{ newer, older }, "Keja");

        Assert.True(newer.CreatedAt > older.CreatedAt);
        Assert.Same(newer, user.GetNewestOrder());
    }

    [Fact]
    public void HistoryPayments_CanStorePaymentsLinkedToTheSameUser()
    {
        Order order = TestData.CreateOrder();
        User user = new User(new List<Order>{ order }, "Keja");
        Payment payment = new Payment(order, 1, PaymentStatus.Success,
            order.TotalPrice, PaymentMethod.Alipay, user);

        user.HistoryPayments.Add(payment);

        Assert.Same(payment, Assert.Single(user.HistoryPayments));
        Assert.Same(user, payment.User);
        Assert.Same(order, payment.Order);
    }

    [Fact]
    public void GetLowestPayment_ReturnsLowestAmountFromUnsortedHistory()
    {
        Order order = TestData.CreateOrder();
        User user = new User(new List<Order>{ order }, "Keja");
        Payment high = new Payment(order, 1, PaymentStatus.Pending, 20m, PaymentMethod.Alipay, user);
        Payment low = new Payment(order, 2, PaymentStatus.Success, 5m, PaymentMethod.CreditCard, user);
        user.HistoryPayments.Add(high);
        user.HistoryPayments.Add(low);

        Assert.Same(low, user.GetLowestPayment());
    }

    [Fact]
    public void GetNewestPayment_UsesCreationTimeRatherThanAmountOrListPosition()
    {
        Order order = TestData.CreateOrder();
        User user = new User(new List<Order>{ order }, "Keja");
        Payment older = new Payment(order, 1, PaymentStatus.Pending, 20m, PaymentMethod.Alipay, user);
        Thread.Sleep(20);
        Payment newer = new Payment(order, 2, PaymentStatus.Success, 5m, PaymentMethod.CreditCard, user);
        user.HistoryPayments.Add(newer);
        user.HistoryPayments.Add(older);

        Assert.True(newer.CreatedAt > older.CreatedAt);
        Assert.Same(newer, user.GetNewestPayment());
    }

    [Fact]
    public void GetMoreThanTen_ExcludesBoundaryAndPreservesPaymentObjectsAndHistory()
    {
        Order order = TestData.CreateOrder();
        User user = new User(new List<Order>{ order }, "Keja");
        Payment below = new Payment(order, 1, PaymentStatus.Pending, 5m, PaymentMethod.Alipay, user);
        Payment boundary = new Payment(order, 2, PaymentStatus.Failed, 10m, PaymentMethod.CreditCard, user);
        Payment above = new Payment(order, 3, PaymentStatus.Success, 10.01m, PaymentMethod.BankTransfer, user);
        Payment high = new Payment(order, 4, PaymentStatus.Success, 20m, PaymentMethod.Alipay, user);
        user.HistoryPayments.AddRange(new Payment[]{ high, boundary, below, above });

        List<Payment> result = Assert.IsType<List<Payment>>(user.GetMoreThanTen());

        Assert.Equal(new Payment[]{ high, above }, result);
        Assert.Equal(new Payment[]{ high, boundary, below, above }, user.HistoryPayments);
    }

    [Fact]
    public void EmptyHistory_QueriesReturnNullOrEmptyList()
    {
        User user = new User(new List<Order>(), "Keja");

        Assert.Null(user.GetMostExpensiveOrder());
        Assert.Null(user.GetNewestOrder());
        Assert.Null(user.GetLowestPayment());
        Assert.Null(user.GetNewestPayment());
        Assert.Empty(Assert.IsType<List<Payment>>(user.GetMoreThanTen()));
    }
}
