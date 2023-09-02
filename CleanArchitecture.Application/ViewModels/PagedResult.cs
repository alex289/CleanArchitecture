using System;
using System.Collections.Generic;

namespace CleanArchitecture.Application.ViewModels;

public sealed class PagedResult<T>
{
    public int Count { get; init; }

    public IList<T> Items { get; init; } = Array.Empty<T>();

    public int Page { get; init; }
    public int PageSize { get; init; }

    public PagedResult(int count, IList<T> items, int page, int pageSize)
    {
        Count = count;
        Items = items;
        Page = page;
        PageSize = pageSize;
    }

    // used by json deserializer
    private PagedResult()
    {
    }

    public static PagedResult<T> Empty()
    {
        return new PagedResult<T>
        {
            Count = 0,
            Items = Array.Empty<T>(),
            Page = 1,
            PageSize = 10
        };
    }
}