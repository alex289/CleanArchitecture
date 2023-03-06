using CleanArchitecture.Infrastructure.Database;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Infrastructure.Tests.Fixtures;

public static class UnitOfWorkTestFixture
{
    public static UnitOfWork<ApplicationDbContext> GetUnitOfWork(
        ApplicationDbContext dbContext,
        ILogger<UnitOfWork<ApplicationDbContext>> logger)
    {
        var unitOfWork = new UnitOfWork<ApplicationDbContext>(dbContext, logger);

        return unitOfWork;
    }
}