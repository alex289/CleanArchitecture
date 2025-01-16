using System.Collections.Generic;
using System.Linq;
using CleanArchitecture.Domain.Commands;
using FluentValidation;
using Shouldly;

namespace CleanArchitecture.Domain.Tests;

public class ValidationTestBase<TCommand, TValidation>
    where TCommand : CommandBase
    where TValidation : AbstractValidator<TCommand>
{
    private readonly TValidation _validation;

    protected ValidationTestBase(TValidation validation)
    {
        _validation = validation;
    }

    protected void ShouldBeValid(TCommand command)
    {
        var result = _validation.Validate(command);

        result.IsValid.ShouldBeTrue();
        result.Errors.ShouldBeEmpty();
    }

    protected void ShouldHaveSingleError(
        TCommand command,
        string expectedCode)
    {
        var result = _validation.Validate(command);

        result.IsValid.ShouldBeFalse();

        result.Errors.Count.ShouldBe(1);

        result.Errors.First().ErrorCode.ShouldBe(expectedCode);
    }

    protected void ShouldHaveSingleError(
        TCommand command,
        string expectedCode,
        string expectedMessage)
    {
        var result = _validation.Validate(command);

        result.IsValid.ShouldBeFalse();

        result.Errors.Count.ShouldBe(1);

        result.Errors.First().ErrorCode.ShouldBe(expectedCode);
        result.Errors.First().ErrorMessage.ShouldBe(expectedMessage);
    }

    protected void ShouldHaveExpectedErrors(
        TCommand command,
        params KeyValuePair<string, string>[] expectedErrors)
    {
        var result = _validation.Validate(command);

        result.IsValid.ShouldBeFalse();
        result.Errors.Count.ShouldBe(expectedErrors.Length);

        foreach (var error in expectedErrors)
        {
            result.Errors
                .Count(validation => validation.ErrorCode == error.Key && validation.ErrorMessage == error.Value)
                .ShouldBe(1);
        }
    }

    protected void ShouldHaveExpectedErrors(
        TCommand command,
        params string[] expectedErrors)
    {
        var result = _validation.Validate(command);

        result.IsValid.ShouldBeFalse();
        result.Errors.Count.ShouldBe(expectedErrors.Length);

        foreach (var error in expectedErrors)
        {
            result.Errors
                .Count(validation => validation.ErrorCode == error)
                .ShouldBe(1);
        }
    }
}