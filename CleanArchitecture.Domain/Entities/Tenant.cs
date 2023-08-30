using System;
using System.Collections.Generic;

namespace CleanArchitecture.Domain.Entities;

public class Tenant : Entity
{
    public Tenant(
        Guid id,
        string name) : base(id)
    {
        Name = name;
    }

    public string Name { get; private set; }

    public virtual ICollection<User> Users { get; private set; } = new HashSet<User>();

    public void SetName(string name)
    {
        Name = name;
    }
}