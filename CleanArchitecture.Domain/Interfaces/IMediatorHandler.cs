using System.Threading.Tasks;
using CleanArchitecture.Domain.Commands;
using CleanArchitecture.Shared.Events;
using MediatR;

namespace CleanArchitecture.Domain.Interfaces;

public interface IMediatorHandler
{
    Task RaiseEventAsync<T>(T @event) where T : DomainEvent;

    Task SendCommandAsync<T>(T command) where T : CommandBase;

    Task<TResponse> QueryAsync<TResponse>(IRequest<TResponse> query);
}