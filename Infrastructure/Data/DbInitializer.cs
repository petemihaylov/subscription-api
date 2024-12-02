using SubscriptionApi.Models;

namespace SubscriptionApi.Infrastructure.Data;

public static class DbInitializer
{
    public static void Initialize(SubscriptionApi.Infrastructure.Data.AppDbContext context)
    {
        if (context.Services.Any())
            return;

        var services = new Service[]
        {
            new() { Id = 1, Name = "eLearning Portal", MonthlyPrice = 10 },
            new() { Id = 2, Name = "Health&Lifestyle", MonthlyPrice = 12 },
            new() { Id = 3, Name = "Gaming+ Catalogue", MonthlyPrice = 15 },
            new() { Id = 4, Name = "Magazines and News", MonthlyPrice = 8 }
        };

        context.Services.AddRange(services);
        context.SaveChanges();
    }
}