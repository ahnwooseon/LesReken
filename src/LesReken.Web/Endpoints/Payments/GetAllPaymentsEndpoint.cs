using FastEndpoints;
using LesReken.Application.Interfaces;
using LesReken.Domain.Models;

namespace LesReken.Web.Endpoints.Payments;

public class GetAllPaymentsEndpoint(IPaymentService paymentService) : EndpointWithoutRequest<IReadOnlyCollection<Payment>>
{
    public override void Configure()
    {
        Get("/api/payments");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var result = await paymentService.GetAllAsync(ct);
        if (result.IsSuccess)
            await Send.OkAsync(result.Value, cancellation: ct);
        else
            await Send.ErrorsAsync(500, ct);
    }
}
