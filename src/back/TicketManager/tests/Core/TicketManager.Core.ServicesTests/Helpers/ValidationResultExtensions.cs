using FluentAssertions;
using FluentValidation.Results;

namespace TicketManager.Core.ServicesTests.Helpers;

public static class ValidationResultExtensions
{
    public static void EnsureCorrectError(this ValidationResult validationResult, int errorCode)
    {
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Select(e => int.Parse(e.ErrorCode)).Should().Contain(errorCode);
    }
}
