using FluentValidation;
using Flow.Tasks.Api.DTOs;

namespace Flow.Tasks.Api.Validation;

public class UpdateStatusValidator : AbstractValidator<UpdateTaskStatusRequest>
{
    public UpdateStatusValidator()
    {
        RuleFor(x => x.Status).InclusiveBetween(0, 3);
        RuleFor(x => x.RowVersion)
            .NotEmpty()
            .Must(IsBase64OrHex).WithMessage("RowVersion doit être en Base64 ou '0x...'.");
    }

    private static bool IsBase64OrHex(string s)
    {
        if (string.IsNullOrWhiteSpace(s)) return false;
        if (s.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
        {
            var hex = s[2..];
            return hex.Length % 2 == 0 && hex.All(Uri.IsHexDigit);
        }
        try { _ = Convert.FromBase64String(s); return true; }
        catch { return false; }
    }
}
