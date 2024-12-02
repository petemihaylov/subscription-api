using Microsoft.EntityFrameworkCore;
using SubscriptionApi.Infrastructure.Data;
using SubscriptionApi.Models;

namespace SubscriptionApi.Services;

public class DiscountService : IDiscountService
{
    private readonly AppDbContext _context;

    public DiscountService(AppDbContext context)
    {
        _context = context;
    }

    public List<DiscountDetail> CalculateDiscounts(List<Subscription> subscriptions)
    {
        var discounts = new List<DiscountDetail>();

        // Service Pair Promotion
        if (HasHealthAndMagazinesSubscription(subscriptions))
        {
            var healthSubscription = subscriptions.First(s => s.Service!.Name == "Health&Lifestyle");
            discounts.Add(new DiscountDetail
            {
                DiscountName = "Service Pair Promotion",
                Amount = healthSubscription.Service!.MonthlyPrice
            });
        }

        // Quantity-Based Discount
        if (subscriptions.Count >= 3)
        {
            var totalCost = subscriptions.Sum(s => s.Service!.MonthlyPrice * s.DurationMonths);
            discounts.Add(new DiscountDetail
            {
                DiscountName = "Quantity-Based Discount (10%)",
                Amount = totalCost * 0.1m
            });
        }

        // Bundled Discount
        if (HasGamingAndElearningSubscription(subscriptions))
        {
            discounts.Add(new DiscountDetail
            {
                DiscountName = "Bundled Discount",
                Amount = 5
            });
        }

        // Upfront Subscription Bonus
        foreach (var subscription in subscriptions.Where(s => s.DurationMonths >= 5))
        {
            discounts.Add(new DiscountDetail
            {
                DiscountName = $"Upfront Subscription Bonus ({subscription.Service!.Name})",
                Amount = subscription.Service!.MonthlyPrice
            });
        }

        return discounts;
    }

    private bool HasHealthAndMagazinesSubscription(List<Subscription> subscriptions)
    {
        return subscriptions.Any(s => s.Service!.Name == "Health&Lifestyle") &&
               subscriptions.Any(s => s.Service!.Name == "Magazines and News");
    }

    private bool HasGamingAndElearningSubscription(List<Subscription> subscriptions)
    {
        return subscriptions.Any(s => s.Service!.Name == "Gaming+ Catalogue") &&
               subscriptions.Any(s => s.Service!.Name == "eLearning Portal");
    }
}