using FluentValidation;
using MediatR;
using MedicalScheduling.Domain.Primitives;

namespace MedicalScheduling.Application.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
  private readonly IEnumerable<IValidator<TRequest>> _validators;

  public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
      => _validators = validators;

  public async Task<TResponse> Handle(
      TRequest request,
      RequestHandlerDelegate<TResponse> next,
      CancellationToken ct)
  {
    if (!_validators.Any())
      return await next();

    var context = new ValidationContext<TRequest>(request);

    var failures = _validators
        .Select(v => v.Validate(context))
        .SelectMany(r => r.Errors)
        .Where(f => f is not null)
        .ToList();

    if (failures.Count == 0)
      return await next();

    var error = new Error(
        "Validation.Failed",
        string.Join("; ", failures.Select(f => f.ErrorMessage)));

    // Suporta tanto Result quanto Result<T>
    var resultType = typeof(TResponse);

    if (resultType == typeof(Result))
      return (TResponse)(object)Result.Failure(error);

    var innerType = resultType.GetGenericArguments()[0];
    var failureMethod = typeof(Result)
        .GetMethods()
        .First(m => m.Name == "Failure" && m.IsGenericMethod)
        .MakeGenericMethod(innerType);

    return (TResponse)failureMethod.Invoke(null, [error])!;
  }
}