using SubscriptionApi.Models;

namespace SubscriptionApi.Services;

public interface ISubscriptionService
{
    Task<Subscription> Subscribe(string customerPhoneNumber, int serviceId, int durationMonths);
    Task Unsubscribe(string customerPhoneNumber, int serviceId);
    Task<SubscriptionSummary> GetSubscriptionSummary(string customerPhoneNumber);
}