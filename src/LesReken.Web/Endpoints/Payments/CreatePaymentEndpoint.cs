using FastEndpoints;
using LesReken.Application.DTOs.Payments;
using LesReken.Application.Interfaces;
using LesReken.Domain.Models;

namespace LesReken.Web.Endpoints.Payments;

public class CreatePaymentEndpoint(IPaymentService paymentService) : Endpoint<CreatePaymentRequest, Payment>
{
    public override void Configure()
    {
        Post("/api/payments");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreatePaymentRequest req, CancellationToken ct)
    {
        var result = await paymentService.AddPaymentAsync(req.StudentId, req.Amount, req.PaymentDate, req.Method, req.Reference, ct);
        
        if (result.IsSuccess)
            await Send.OkAsync(result.Value, cancellation: ct);
        else
        {
            AddError(result.Errors[0].Message);
            await Send.ErrorsAsync(400, ct);
        }
    }
}
