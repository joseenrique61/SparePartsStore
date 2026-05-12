using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPSAPI.Data;
using SPSAPI.Services;
using SPSModels.Models;

[ApiController]
[Route("api/[controller]")]
public class PaymentController(IVaultKmsService kmsService, ApplicationDBContext context, ILogger<PaymentController> logger) : ControllerBase
{
    [HttpPost("process")]
    public async Task<IActionResult> ProcessPayment([FromBody] CallbackRequest request)
    {
        try
        {
            string token = Uri.UnescapeDataString(request.Token);
            // 1. DESENCRIPTAR: Usamos el KMS para leer lo que mandó el Sistema B
            var confirmation = await kmsService.DecryptPayloadAsync<PaymentConfirmation>(token);

            if (confirmation.Status != "PAID_SUCCESS")
            {
                return BadRequest("Payment not successful.");
            }

            // 2. PROCESAR: Aquí usarías el confirmation.LocalUserId o confirmation.OrderId
            // para marcar la compra como pagada en tu SQL Server.
            PurchaseOrder? purchaseOrder = await context.PurchaseOrders
                .Include(p => p.Client)
                .Include(p => p.Orders)
                .FirstOrDefaultAsync(p => p.Client!.UserId == confirmation.UserId && p.PurchaseCompleted == false);

            if (purchaseOrder == null)
            {
                return BadRequest("The specified user doesn't have any active purchase order.");
            }

            purchaseOrder.PurchaseCompleted = true;
            purchaseOrder.Client = null;
            context.Update(purchaseOrder);
            await context.SaveChangesAsync();

            List<SparePart> spareParts = (await context.SparePart.ToListAsync())!;
            foreach (SparePart sparePart in spareParts)
            {
                Order? order = purchaseOrder.Orders.FirstOrDefault(o => o.SparePartId == sparePart.Id);
                if (order == null)
                {
                    continue;
                }

                sparePart.Stock -= order.Amount;
                context.Update(sparePart);
                await context.SaveChangesAsync();
            }

            logger.LogInformation($"Pago confirmado para Usuario Local: {confirmation.UserId}");

            return Ok(new { message = "Sistema A actualizado correctamente" });
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return BadRequest("Error de integridad en la trama del KMS");
        }
    }


    [HttpPost("generate-redirect-url")]
    public async Task<IActionResult> GenerateRedirectUrl([FromBody] OrderRequest orderRequest)
    {
        PurchaseOrder? purchaseOrder = await context.PurchaseOrders
            .Include(p => p.Client)
            .Include(p => p.Orders)
            .ThenInclude(o => o.SparePart)
            .FirstOrDefaultAsync(p => p.Client!.UserId == orderRequest.UserId && p.PurchaseCompleted == false);

        if (purchaseOrder == null)
        {
            return BadRequest("The provided user doesn't have any open purchase orders.");
        }

        // 1. Datos que queremos proteger (Trama sensible)
        var payload = new
        {
            amount = purchaseOrder.Orders.Sum(o => o.Amount * o.SparePart!.Price),
            orderId = $"SPS-{Guid.NewGuid().ToString()[..8].ToUpper()}",
            store = "Ferretería SPS",
            timestamp = DateTime.UtcNow,
            redirectUrl = "http://localhost:5228/",
            userId = purchaseOrder.Client!.UserId
        };

        // 2. ENCRIPTACIÓN KMS: Invocamos al tercero (Vault)
        string ciphertext = await kmsService.EncryptPayloadAsync(payload);

        // 3. CONSTRUIR URL REDIRECCIÓN:
        // Usar Uri.EscapeDataString porque el ciphertext tiene caracteres como ':' y '/'
        // que rompen la URL si no se escapan.
        string encodedToken = Uri.EscapeDataString(ciphertext);
        string systemBUrl = $"http://localhost:5173/checkout?token={encodedToken}";

        return Ok(new { redirectUrl = systemBUrl });
    }
}

public class OrderRequest { public int UserId { get; set; } }

public class PaymentRequest { public string Token { get; set; } }

public class CallbackRequest { public string Token { get; set; } }

public class PaymentConfirmation
{
    public int UserId { get; set; }
    public string Status { get; set; }
}