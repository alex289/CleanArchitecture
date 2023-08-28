using System;

namespace CleanArchitecture.Application.ViewModels.Tenants;

public sealed record UpdateTenantViewModel(
    Guid Id,
    string Name);