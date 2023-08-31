using CleanArchitecture.Application.ViewModels;
using CleanArchitecture.Application.ViewModels.Users;
using MediatR;

namespace CleanArchitecture.Application.Queries.Users.GetAll;

public sealed record GetAllUsersQuery(PageQuery Query, string SearchTerm = "") :
    IRequest<PagedResult<UserViewModel>>;