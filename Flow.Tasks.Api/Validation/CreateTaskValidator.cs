using FluentValidation;
using Flow.Tasks.Api.DTOs;

namespace Flow.Tasks.Api.Validation;

public class CreateTaskValidator : AbstractValidator<CreateTaskRequest>
{
    public CreateTaskValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(256);
        RuleFor(x => x.Description).MaximumLength(4000).When(x => x.Description != null);
        RuleFor(x => x.AssignedTo).MaximumLength(256).When(x => !string.IsNullOrWhiteSpace(x.AssignedTo));
    }
}
