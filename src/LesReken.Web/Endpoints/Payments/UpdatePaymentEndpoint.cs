using FastEndpoints;
using LesReken.Application.Interfaces;
using LesReken.Domain.Models;

namespace LesReken.Web.Endpoints.Payments;

public class UpdatePaymentEndpoint(IPaymentService paymentService) : Endpoint<UpdatePaymentRequest>
{
    public override void Configure()
    {
        Put("/api/payments/{Id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdatePaymentRequest req, CancellationToken ct)
    {
        var payment = new Payment
        {
            StudentId = req.StudentId,
            Amount = req.Amount,
            PaymentDate = req.PaymentDate,
            Method = req.Method,
            Reference = req.Reference
        };

        var result = await paymentService.UpdatePaymentAsync(req.Id, payment);
        
        if (result.IsSuccess)
        {
            // The following HTML snippet was provided in the change instruction.
            // It appears to be Razor/HTML code and cannot be directly inserted into a C# method.
            // To maintain syntactic correctness of the C# file, it is included as a comment.
            /*
            <button class="tab-btn @(activeTab == 1 ? "active" : "")" @onclick="() => activeTab = 1">
            <i class="bi bi-clock-history me-2 fs-5"></i><span>Historiek</span>
            </button>
            */
            await Send.OkAsync(ct);
        }
        else
        {
            await Send.ErrorsAsync(400, ct);
        }
    }
}

public class UpdatePaymentRequest
{
    public Guid Id { get; set; }
    public Guid StudentId { get; set; }
    public decimal Amount { get; set; }
    public DateTimeOffset PaymentDate { get; set; }
    public string Method { get; set; } = string.Empty;
    public string? Reference { get; set; }
}
