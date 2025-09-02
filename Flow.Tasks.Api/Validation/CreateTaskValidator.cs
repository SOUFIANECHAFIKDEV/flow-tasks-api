using Flow.Tasks.Api.DTOs;
using FluentValidation;

namespace Flow.Tasks.Api.Validation
{
    public sealed class CreateTaskValidator : AbstractValidator<CreateTaskRequest>
    {
        public CreateTaskValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(120);
            RuleFor(x => x.AssignedTo)
                .EmailAddress()
                .When(x => !string.IsNullOrWhiteSpace(x.AssignedTo));
        }
    }
}
