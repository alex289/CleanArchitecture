using System;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Infrastructure.Database;
using CleanArchitecture.Infrastructure.Tests.Fixtures;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CleanArchitecture.Infrastructure.Tests;

public sealed class UnitOfWorkTests
{
    [Fact]
    public async Task Should_Commit_Async_Returns_True()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>();
        var dbContextMock = new Mock<ApplicationDbContext>(options.Options);
        var loggerMock = new Mock<ILogger<UnitOfWork<ApplicationDbContext>>>();
        
        dbContextMock
            .Setup(x => x.SaveChangesAsync(CancellationToken.None))
            .Returns(Task.FromResult(1));
        
        var unitOfWork = UnitOfWorkTestFixture.GetUnitOfWork(dbContextMock.Object, loggerMock.Object);
        
        var result = await unitOfWork.CommitAsync();

        result.Should().BeTrue();
    }
    
    [Fact]
    public async Task Should_Commit_Async_Returns_False()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>();
        var dbContextMock = new Mock<ApplicationDbContext>(options.Options);
        var loggerMock = new Mock<ILogger<UnitOfWork<ApplicationDbContext>>>();

        dbContextMock
            .Setup(x => x.SaveChangesAsync(CancellationToken.None))
            .Throws(new DbUpdateException("Boom", new Exception("it broke")));

        var unitOfWork = UnitOfWorkTestFixture.GetUnitOfWork(dbContextMock.Object, loggerMock.Object);

        var result = await unitOfWork.CommitAsync();
            
        result.Should().BeFalse();
    }

    [Fact]
    public async Task Should_Throw_Exception_When_Commiting_With_DbUpdateException()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>();
        var dbContextMock = new Mock<ApplicationDbContext>(options.Options);
        var loggerMock = new Mock<ILogger<UnitOfWork<ApplicationDbContext>>>();

        dbContextMock
            .Setup(x => x.SaveChangesAsync(CancellationToken.None))
            .Throws(new Exception("boom"));

        var unitOfWork = UnitOfWorkTestFixture.GetUnitOfWork(dbContextMock.Object, loggerMock.Object);

        Func<Task> throwsAction = async () => await unitOfWork.CommitAsync();

        await throwsAction.Should().ThrowAsync<Exception>();
    }
}