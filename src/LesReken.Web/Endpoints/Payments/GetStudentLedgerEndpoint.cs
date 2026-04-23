using FastEndpoints;
using LesReken.Application.DTOs.Payments;
using LesReken.Application.Interfaces;

namespace LesReken.Web.Endpoints.Payments;

public class GetStudentLedgerEndpoint(IPaymentService paymentService) : EndpointWithoutRequest<IReadOnlyCollection<StudentLedgerEntry>>
{
    public override void Configure()
    {
        Get("/api/payments/students/{studentId}/ledger");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        Guid studentId = Route<Guid>("studentId");
        var result = await paymentService.GetStudentLedgerAsync(studentId, ct);
        
        if (result.IsSuccess)
            await Send.OkAsync(result.Value, cancellation: ct);
        else
            await Send.ErrorsAsync(400, ct);
    }
}
