using FieldUp.Infrastructure.Configurations;
using FieldUp.Infrastructure.Services;

namespace FieldUp.Infrastructure.Extensions;

public static class SecretProviderEx
{
    public static string GetRequiredSecret(this ISecretProvider provider, string name)
    {
        var secret = provider.GetSecretAsync(name).GetAwaiter().GetResult();
        return string.IsNullOrEmpty(secret) ? 
            throw new InvalidOperationException($"Required secret '{name}' is missing.") : secret;
    }
}