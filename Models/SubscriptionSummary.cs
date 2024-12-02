namespace SubscriptionApi.Models;

public class SubscriptionSummary
{
    public string CustomerPhoneNumber { get; set; } = string.Empty;
    public List<SubscriptionDetail> Subscriptions { get; set; } = new();
    public decimal TotalCostBeforeDiscounts { get; set; }
    public decimal TotalDiscounts { get; set; }
    public decimal FinalCost { get; set; }
    public List<DiscountDetail> AppliedDiscounts { get; set; } = new();
}

public class SubscriptionDetail
{
    public string ServiceName { get; set; } = string.Empty;
    public int DurationMonths { get; set; }
    public decimal MonthlyPrice { get; set; }
}

public class DiscountDetail
{
    public string DiscountName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}