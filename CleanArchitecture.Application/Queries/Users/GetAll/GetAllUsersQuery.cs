using System.Collections.Generic;
using CleanArchitecture.Application.ViewModels.Users;
using MediatR;

namespace CleanArchitecture.Application.Queries.Users.GetAll;

public sealed record GetAllUsersQuery : IRequest<IEnumerable<UserViewModel>>;
