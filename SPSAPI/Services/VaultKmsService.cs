using System.Text;
using System.Text.Json;

namespace SPSAPI.Services;

public interface IVaultKmsService
{
    Task<string> EncryptPayloadAsync(object payload);

    Task<T> DecryptPayloadAsync<T>(string cyphertext);
}

public class VaultKmsService : IVaultKmsService
{
    private readonly HttpClient _httpClient;
    private const string VaultToken = "my-root-token"; // El que pusimos en setup.sh
    private const string VaultAddr = "http://localhost:8200"; // URL de Vault en Docker

    public VaultKmsService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        // Importante: Vault requiere el token en este header
        _httpClient.DefaultRequestHeaders.Add("X-Vault-Token", VaultToken);
    }

    public async Task<string> EncryptPayloadAsync(object payload)
    {
        // 1. Convertir el objeto (monto, orden, etc) a JSON
        var json = JsonSerializer.Serialize(payload);

        // 2. Vault REQUIERE que el texto plano esté en Base64
        var base64Plaintext = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));

        // 3. Preparar la petición para el motor Transit
        var requestBody = new { plaintext = base64Plaintext };

        var response = await _httpClient.PostAsJsonAsync(
            $"{VaultAddr}/v1/transit/encrypt/payments-key",
            requestBody
        );

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<VaultResponse>();

        // El Ciphertext se ve así: "vault:v1:asdqwe..."
        return result?.Data?.Ciphertext ?? throw new Exception("Error al obtener cifrado del KMS");
    }

    public async Task<T> DecryptPayloadAsync<T>(string ciphertext)
    {
        var requestBody = new { ciphertext = ciphertext };
        var response = await _httpClient.PostAsJsonAsync($"{VaultAddr}/v1/transit/decrypt/payments-key", requestBody);

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<VaultDecryptResponse>();

        // Vault devuelve Base64, lo decodificamos a string
        var base64Plaintext = result?.Data?.Plaintext ?? throw new Exception("Error KMS");
        var json = Encoding.UTF8.GetString(Convert.FromBase64String(base64Plaintext));

        return JsonSerializer.Deserialize<T>(json)!;
    }

    private class VaultDecryptResponse { public DecryptData Data { get; set; } }
    private class DecryptData { public string Plaintext { get; set; } }

    private class VaultResponse { public VaultData Data { get; set; } }
    private class VaultData { public string Ciphertext { get; set; } }
}