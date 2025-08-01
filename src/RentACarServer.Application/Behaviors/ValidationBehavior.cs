﻿using FluentValidation;
using FluentValidation.Results;
using TS.MediatR;

namespace RentACarServer.Application.Behaviors;
public sealed class ValidationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        #region Asenkron hali
        // if (!_validators.Any())
        // {
        //     return await next();
        // }

        //var context = new ValidationContext<TRequest>(request);

        // var validationTasks = _validators
        //     .Select(validator => validator.ValidateAsync(context, cancellationToken));

        // var validationResults = await Task.WhenAll(validationTasks);

        // var failures = validationResults
        //     .SelectMany(result => result.Errors)
        //     .Where(failure => failure is not null) // C# 9.0+ desen eşleştirme ile null kontrolü
        //     .ToList();

        // if (failures.Count != 0)
        // {
        //     throw new ValidationException(failures);
        // }

        // return await next();

        #endregion

        #region Senkron hali
        if (!_validators.Any())
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);

        var errorDictionary = _validators
            .Select(s => s.Validate(context))
            .SelectMany(s => s.Errors)
            .Where(s => s != null)
            .GroupBy(
            s => s.PropertyName,
            s => s.ErrorMessage, (propertyName, errorMessage) => new
            {
                Key = propertyName,
                Values = errorMessage.Distinct().ToArray()
            })
            .ToDictionary(s => s.Key, s => s.Values[0]);

        if (errorDictionary.Any())
        {
            var errors = errorDictionary.Select(s => new ValidationFailure
            {
                PropertyName = s.Value,
                ErrorCode = s.Key
            });
            throw new ValidationException(errors);
        }

        return await next();
        #endregion


    }
}
