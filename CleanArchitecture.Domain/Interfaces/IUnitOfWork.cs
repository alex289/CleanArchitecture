using System;
using System.Threading.Tasks;

namespace CleanArchitecture.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    public Task<bool> CommitAsync();
}