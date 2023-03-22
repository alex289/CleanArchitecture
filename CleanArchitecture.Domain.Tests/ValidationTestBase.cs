using System.Collections.Generic;
using System.Linq;
using CleanArchitecture.Domain.Commands;
using FluentAssertions;
using FluentValidation;

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

        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    protected void ShouldHaveSingleError(
        TCommand command,
        string expectedCode)
    {
        var result = _validation.Validate(command);

        result.IsValid.Should().BeFalse();

        result.Errors.Count.Should().Be(1);

        result.Errors.First().ErrorCode.Should().Be(expectedCode);
    }

    protected void ShouldHaveSingleError(
        TCommand command,
        string expectedCode,
        string expectedMessage)
    {
        var result = _validation.Validate(command);

        result.IsValid.Should().BeFalse();

        result.Errors.Count.Should().Be(1);

        result.Errors.First().ErrorCode.Should().Be(expectedCode);
        result.Errors.First().ErrorMessage.Should().Be(expectedMessage);
    }

    protected void ShouldHaveExpectedErrors(
        TCommand command,
        params KeyValuePair<string, string>[] expectedErrors)
    {
        var result = _validation.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Count.Should().Be(expectedErrors.Length);

        foreach (var error in expectedErrors)
        {
            result.Errors
                .Count(validation => validation.ErrorCode == error.Key && validation.ErrorMessage == error.Value)
                .Should()
                .Be(1);
        }
    }

    protected void ShouldHaveExpectedErrors(
        TCommand command,
        params string[] expectedErrors)
    {
        var result = _validation.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Count.Should().Be(expectedErrors.Length);

        foreach (var error in expectedErrors)
        {
            result.Errors
                .Count(validation => validation.ErrorCode == error)
                .Should()
                .Be(1);
        }
    }
}