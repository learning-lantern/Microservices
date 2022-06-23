using FluentValidation;
using LearningLantern.Common.Extensions;
using LearningLantern.Common.Responses;
using MediatR;

namespace LearningLantern.ApiGateway.PipelineBehaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Response
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .SelectMany(r => r.Errors)
            .Where(r => r != null)
            .ToList().ToApplicationValidationError();

        if (failures.Any()) return ResponseFactory.CreateFailObject<TResponse>(failures);
        return await next();
    }
}