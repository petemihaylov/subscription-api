using SubscriptionApi.Models;

namespace SubscriptionApi.Infrastructure.Data.Repositories;

/// <summary>
/// Repository interface for managing Service entities.
/// </summary>
public interface IServiceRepository
{
    /// <summary>
    /// Retrieves a service by its unique identifier.
    /// </summary>
    /// <param name="id">The service identifier.</param>
    /// <returns>The service if found; otherwise, null.</returns>
    Task<Service?> GetByIdAsync(int id);

    /// <summary>
    /// Retrieves a service by its name.
    /// </summary>
    /// <param name="name">The service name.</param>
    /// <returns>The service if found; otherwise, null.</returns>
    Task<Service?> GetByNameAsync(string name);
}