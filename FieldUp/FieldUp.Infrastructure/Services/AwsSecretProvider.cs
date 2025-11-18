using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;

namespace FieldUp.Infrastructure.Services;

public interface ISecretProvider
{
    Task<string?> GetSecretAsync(string secretName, CancellationToken ct = default);
}

public class AwsSecretProvider(IAmazonSecretsManager client) : ISecretProvider
{
    public async Task<string?> GetSecretAsync(string secretName, CancellationToken ct = default)
    {
        var response = await client.GetSecretValueAsync(new GetSecretValueRequest
        {
            SecretId = secretName
        }, ct);

        return response.SecretString;
    }
}