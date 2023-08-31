using System;

namespace CleanArchitecture.Domain.Entities;

public abstract class Entity
{
    public Guid Id { get; private set; }
    public bool Deleted { get; private set; }

    protected Entity(Guid id)
    {
        Id = id;
    }

    public void SetId(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException($"{nameof(id)} may not be empty");
        }

        Id = id;
    }

    public void Delete()
    {
        Deleted = true;
    }

    public void Undelete()
    {
        Deleted = false;
    }
}