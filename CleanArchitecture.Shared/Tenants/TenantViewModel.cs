using System;

namespace CleanArchitecture.Shared.Tenants;

public sealed record TenantViewModel(
    Guid Id,
    string Name,
    DateTimeOffset? DeletedAt);