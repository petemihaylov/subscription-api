namespace SubscriptionApi.Models;

public class Subscription
{
    public int Id { get; set; }
    public string CustomerPhoneNumber { get; set; } = string.Empty;
    public int ServiceId { get; set; }
    public Service? Service { get; set; }
    public int DurationMonths { get; set; }
    public DateTime SubscriptionDate { get; set; }
}