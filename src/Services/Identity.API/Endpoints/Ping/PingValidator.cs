using FastEndpoints;

using FluentValidation;

namespace Identity.API.Endpoints.Ping;

internal sealed class PingValidator : Validator<PingRequest>
{
    public PingValidator()
    {
        RuleFor(x => x.Message)
            .NotEmpty()
            .MaximumLength(100);
    }
}
