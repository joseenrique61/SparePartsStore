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
    private readonly ILogger<VaultKmsService> _logger;
    private readonly IConfiguration _configuration;

    public VaultKmsService(HttpClient httpClient, IConfiguration configuration, ILogger<VaultKmsService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _configuration = configuration;
        // Importante: Vault requiere el token en este header
        _httpClient.DefaultRequestHeaders.Add("X-Vault-Token", _configuration.GetSection("Vault:Token").Value);
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
            $"{_configuration.GetSection("Vault:Url").Value}/v1/transit/encrypt/payments-key",
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
        var response = await _httpClient.PostAsJsonAsync($"{_configuration.GetSection("Vault:Url").Value}/v1/transit/decrypt/payments-key", requestBody);

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<VaultDecryptResponse>();

        // Vault devuelve Base64, lo decodificamos a string
        var base64Plaintext = result?.Data?.Plaintext ?? throw new Exception("Error KMS");
        _logger.LogWarning(base64Plaintext);
        var json = Encoding.UTF8.GetString(Convert.FromBase64String(base64Plaintext));
        _logger.LogWarning(json);

        return JsonSerializer.Deserialize<T>(json, options: new()
        {
            PropertyNameCaseInsensitive = true
        })!;
    }

    private class VaultDecryptResponse { public DecryptData Data { get; set; } }
    private class DecryptData { public string Plaintext { get; set; } }

    private class VaultResponse { public VaultData Data { get; set; } }
    private class VaultData { public string Ciphertext { get; set; } }
}