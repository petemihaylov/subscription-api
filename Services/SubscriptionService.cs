using Microsoft.EntityFrameworkCore;
using SubscriptionApi.Infrastructure.Data;
using SubscriptionApi.Models;

namespace SubscriptionApi.Services;

public class SubscriptionService : ISubscriptionService
{
    private readonly AppDbContext _context;
    private readonly IDiscountService _discountService;

    public SubscriptionService(AppDbContext context, IDiscountService discountService)
    {
        _context = context;
        _discountService = discountService;
    }

    public async Task<Subscription> Subscribe(string customerPhoneNumber, int serviceId, int durationMonths)
    {
        var service = await _context.Services.FindAsync(serviceId) 
            ?? throw new ArgumentException("Service not found");

        var existingSubscription = await _context.Subscriptions
            .FirstOrDefaultAsync(s => s.CustomerPhoneNumber == customerPhoneNumber && s.ServiceId == serviceId);

        if (existingSubscription != null)
            throw new InvalidOperationException("Customer already subscribed to this service");

        var subscription = new Subscription
        {
            CustomerPhoneNumber = customerPhoneNumber,
            ServiceId = serviceId,
            DurationMonths = durationMonths,
            SubscriptionDate = DateTime.UtcNow
        };

        _context.Subscriptions.Add(subscription);
        await _context.SaveChangesAsync();

        return subscription;
    }

    public async Task Unsubscribe(string customerPhoneNumber, int serviceId)
    {
        var subscription = await _context.Subscriptions
            .FirstOrDefaultAsync(s => s.CustomerPhoneNumber == customerPhoneNumber && s.ServiceId == serviceId)
            ?? throw new ArgumentException("Subscription not found");

        _context.Subscriptions.Remove(subscription);
        await _context.SaveChangesAsync();
    }

    public async Task<SubscriptionSummary> GetSubscriptionSummary(string customerPhoneNumber)
    {
        var subscriptions = await _context.Subscriptions
            .Include(s => s.Service)
            .Where(s => s.CustomerPhoneNumber == customerPhoneNumber)
            .ToListAsync();

        var summary = new SubscriptionSummary
        {
            CustomerPhoneNumber = customerPhoneNumber,
            Subscriptions = subscriptions.Select(s => new SubscriptionDetail
            {
                ServiceName = s.Service!.Name,
                DurationMonths = s.DurationMonths,
                MonthlyPrice = s.Service.MonthlyPrice
            }).ToList()
        };

        var discounts = _discountService.CalculateDiscounts(subscriptions);
        
        summary.TotalCostBeforeDiscounts = subscriptions.Sum(s => s.Service!.MonthlyPrice * s.DurationMonths);
        summary.AppliedDiscounts = discounts;
        summary.TotalDiscounts = discounts.Sum(d => d.Amount);
        summary.FinalCost = summary.TotalCostBeforeDiscounts - summary.TotalDiscounts;

        return summary;
    }
}