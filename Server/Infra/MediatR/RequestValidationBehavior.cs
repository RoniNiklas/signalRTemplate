using FluentValidation.Results;
using Shared.DTOs;

namespace Server.Infra.MediatR;
public class RequestValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : RequestResult, new()
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public RequestValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        List<ValidationResult> results = new();

        foreach (var validator in _validators)
        {
            results.Add(await validator.ValidateAsync(request, opt => opt.IncludeAllRuleSets(), cancellationToken));
        }

        var failures = results.SelectMany(result => result.Errors)
            .Where(f => f != null)
            .ToList();

        if (failures.Count != 0)
        {
            return new()
            {
                ValidationError = new(failures)
            };
        }

        return await next();
    }
}
