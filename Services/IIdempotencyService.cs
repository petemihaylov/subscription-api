namespace SubscriptionApi.Services;

public interface IIdempotencyService
{
    string GenerateIdempotencyKey();
    bool IsKeyUsed(string key);
    void MarkKeyAsUsed(string key, TimeSpan expirationTime);
}