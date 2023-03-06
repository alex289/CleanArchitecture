using System;
using CleanArchitecture.Application.ViewModels;
using MediatR;

namespace CleanArchitecture.Application.Queries.Users.GetUserById;

public sealed record GetUserByIdQuery(Guid UserId) : IRequest<UserViewModel?>;
