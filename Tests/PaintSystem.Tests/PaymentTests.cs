namespace PaintSystem.Tests;

public class PaymentTests
{
    [Theory]
    [InlineData((int)PaymentStatus.Pending, (int)PaymentMethod.Alipay)]
    [InlineData((int)PaymentStatus.Failed, (int)PaymentMethod.CreditCard)]
    [InlineData((int)PaymentStatus.Success, (int)PaymentMethod.BankTransfer)]
    public void Constructor_SavesAllSuppliedPaymentDetails(int statusValue, int methodValue)
    {
        PaymentStatus status = (PaymentStatus)statusValue;
        PaymentMethod method = (PaymentMethod)methodValue;
        Order order = TestData.CreateOrder();
        User user = new User(new List<Order>{ order }, "Keja");
        Payment payment = new Payment(order, 42, status, 17.25m, method, user);

        Assert.Same(order, payment.Order);
        Assert.Equal(42, payment.PaymentId);
        Assert.Equal(status, payment.PaymentStatus);
        Assert.Equal(17.25m, payment.PaymentAmount);
        Assert.Equal(method, payment.PaymentMethod);
        Assert.Same(user, payment.User);
    }

    [Fact]
    public void Constructor_RecordsPaymentCreationTime()
    {
        Order order = TestData.CreateOrder();
        User user = new User(new List<Order>{ order }, "Keja");
        DateTime before = DateTime.Now;

        Payment payment = new Payment(order, 1, PaymentStatus.Pending,
            10m, PaymentMethod.Alipay, user);

        Assert.InRange(payment.CreatedAt, before, DateTime.Now);
    }
}
