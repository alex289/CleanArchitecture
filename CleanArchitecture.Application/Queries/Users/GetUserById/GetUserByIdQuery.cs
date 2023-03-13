using System;
using CleanArchitecture.Application.ViewModels.Users;
using MediatR;

namespace CleanArchitecture.Application.Queries.Users.GetUserById;

public sealed record GetUserByIdQuery(Guid UserId, bool IsDeleted) : IRequest<UserViewModel?>;
