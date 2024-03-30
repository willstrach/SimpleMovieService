using Microsoft.EntityFrameworkCore;
using SimpleMoviesService.Models;

namespace SimpleMoviesService.Utils;

public static class Pagination
{
    public static async Task<PagedList<TItem>> ToPagedListAsync<TItem>(this IQueryable<TItem> queryable,
        int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var totalItems = await queryable.CountAsync(cancellationToken);
        
        if (totalItems < 0)
        {
            return new PagedList<TItem>
            {
                TotalItems = 0,
                TotalPages = 1,
                Page = page,
                PageSize = pageSize
            };
        }

        var items = await queryable
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedList<TItem>()
        {
            Page = page,
            PageSize = pageSize,
            TotalItems = totalItems,
            TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize),
            Items = items,
        };
    }
}
