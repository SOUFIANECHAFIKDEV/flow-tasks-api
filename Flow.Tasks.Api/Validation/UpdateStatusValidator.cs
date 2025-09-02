using FluentValidation;
using Flow.Tasks.Api.DTOs;

namespace Flow.Tasks.Api.Validation;

public sealed class UpdateStatusValidator : AbstractValidator<UpdateTaskStatusRequest>
{
    public UpdateStatusValidator()
    {
        RuleFor(x => x.RowVersion).NotEmpty();
        RuleFor(x => x.Status).IsInEnum();
    }
}