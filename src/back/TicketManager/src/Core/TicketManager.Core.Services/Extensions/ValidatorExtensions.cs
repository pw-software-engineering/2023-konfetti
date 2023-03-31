using FluentValidation;

namespace TicketManager.Core.Services.Extensions;

public static class ValidatorExtensions
{
    public static IRuleBuilderOptions<T, TProperty> WithCode<T, TProperty>(this IRuleBuilderOptions<T, TProperty> rule, int errorCode)
    {
        return rule.WithErrorCode(errorCode.ToString());
    }
}
