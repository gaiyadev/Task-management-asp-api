namespace TaskManagementAPI.Pagination;

public class PagedResult<T>
{
    public List<T> Data { get; set; } = null!;
    public PaginationMetaData Meta { get; set; } = null!;
    public PaginationLinks Links { get; set; } = null!;
}