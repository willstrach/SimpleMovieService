﻿namespace SimpleMoviesService.Models;

public class PagedList<TItem>
{
    public List<TItem> Items { get; set; } = [];
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }
}
