using SubscriptionApi.Models;

namespace SubscriptionApi.Infrastructure.Factories;

public class SubscriptionFactory : ISubscriptionFactory
{
    public Subscription Create(string customerPhoneNumber, Service service, int durationMonths)
    {
        return new Subscription
        {
            CustomerPhoneNumber = customerPhoneNumber,
            ServiceId = service.Id,
            Service = service,
            DurationMonths = durationMonths,
            SubscriptionDate = DateTime.UtcNow
        };
    }
}