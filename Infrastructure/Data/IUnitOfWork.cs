using SubscriptionApi.Infrastructure.Data.Repositories;

namespace SubscriptionApi.Infrastructure.Data;

/// <summary>
/// Represents a unit of work that coordinates multiple repository operations
/// within a single transaction boundary.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Gets the repository for managing services.
    /// </summary>
    IServiceRepository Services { get; }

    /// <summary>
    /// Gets the repository for managing subscriptions.
    /// </summary>
    ISubscriptionRepository Subscriptions { get; }

    /// <summary>
    /// Saves all pending changes to the database.
    /// </summary>
    /// <returns>The number of affected records.</returns>
    Task<int> SaveChangesAsync();
}