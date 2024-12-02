using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SubscriptionApi.Models;

namespace SubscriptionApi.Infrastructure.Data.Repositories;

public class SubscriptionRepository : Repository<Subscription>, ISubscriptionRepository
{
    public SubscriptionRepository(AppDbContext context) : base(context)
    {
    }

    public override async Task<IEnumerable<Subscription>> FindAllAsync(
        Expression<Func<Subscription, bool>> predicate)
    {
        return await Context.Subscriptions
            .Include(s => s.Service)
            .Where(predicate)
            .ToListAsync();
    }
}