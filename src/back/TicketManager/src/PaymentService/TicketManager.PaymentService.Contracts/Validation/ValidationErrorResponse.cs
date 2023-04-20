namespace TicketManager.PaymentService.Contracts.Validation;

public class ValidationErrorResponse
{
    public List<ValidationError> Errors { get; set; }
}

public class ValidationError
{
    public int ErrorCode { get; set; }
    public string ErrorMessage { get; set; }
}
