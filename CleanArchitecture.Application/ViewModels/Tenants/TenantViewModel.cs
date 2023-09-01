using System;
using System.Collections.Generic;
using System.Linq;
using CleanArchitecture.Application.ViewModels.Users;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.ViewModels.Tenants;

public sealed class TenantViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public IEnumerable<UserViewModel> Users { get; set; } = new List<UserViewModel>();

    public static TenantViewModel FromTenant(Tenant tenant)
    {
        return new TenantViewModel
        {
            Id = tenant.Id,
            Name = tenant.Name,
            Users = tenant.Users.Select(UserViewModel.FromUser).ToList()
        };
    }
}