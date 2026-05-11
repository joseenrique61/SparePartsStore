using Microsoft.AspNetCore.Mvc;
using SPSAPI.Services;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly IVaultKmsService _kmsService;

    public PaymentController(IVaultKmsService kmsService)
    {
        _kmsService = kmsService;
    }

    [HttpPost("process")]
    public async Task<IActionResult> ProcessPayment([FromBody] OrderRequest order)
    {
        // 1. Datos que queremos proteger (Trama sensible)
        var payload = new
        {
            amount = order.Total,
            orderId = $"SPS-{Guid.NewGuid().ToString()[..8].ToUpper()}",
            store = "Ferretería SPS",
            timestamp = DateTime.UtcNow
        };

        // 2. ENCRIPTACIÓN KMS: Invocamos al tercero (Vault)
        string ciphertext = await _kmsService.EncryptPayloadAsync(payload);

        // 3. CONSTRUIR URL REDIRECCIÓN:
        // ¡VITAL! Usar Uri.EscapeDataString porque el ciphertext tiene caracteres como ':' y '/'
        // que rompen la URL si no se escapan.
        string encodedToken = Uri.EscapeDataString(ciphertext);
        string systemBUrl = $"http://localhost:5173/checkout?token={encodedToken}";

        return Ok(new { redirectUrl = systemBUrl });
    }
}

public class OrderRequest { public decimal Total { get; set; } }