using FastEndpoints;
using LesReken.Application.Interfaces;

namespace LesReken.Web.Endpoints.Payments;

public class GetStudentBalanceEndpoint(IPaymentService paymentService) : EndpointWithoutRequest<decimal>
{
    public override void Configure()
    {
        Get("/api/payments/students/{studentId}/balance");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        Guid studentId = Route<Guid>("studentId");
        var result = await paymentService.GetStudentBalanceAsync(studentId, ct);
        
        if (result.IsSuccess)
            await Send.OkAsync(result.Value, cancellation: ct);
        else
            await Send.ErrorsAsync(400, ct);
    }
}
