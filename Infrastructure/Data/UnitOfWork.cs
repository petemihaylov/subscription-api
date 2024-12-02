using SubscriptionApi.Infrastructure.Data.Repositories;

namespace SubscriptionApi.Infrastructure.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private IServiceRepository? _serviceRepository;
    private ISubscriptionRepository? _subscriptionRepository;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public IServiceRepository Services => 
        _serviceRepository ??= new ServiceRepository(_context);

    public ISubscriptionRepository Subscriptions => 
        _subscriptionRepository ??= new SubscriptionRepository(_context);

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}