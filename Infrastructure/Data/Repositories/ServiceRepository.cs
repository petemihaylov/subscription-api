using Microsoft.EntityFrameworkCore;
using SubscriptionApi.Models;

namespace SubscriptionApi.Infrastructure.Data.Repositories;

public class ServiceRepository : Repository<Service>, IServiceRepository
{
    public ServiceRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Service?> GetByNameAsync(string name)
    {
        return await Context.Services
            .FirstOrDefaultAsync(s => s.Name == name);
    }
}