namespace TaskManagementAPI.Pagination;

public class PaginationLinks
{
    public string First { get; set; }
    public string Previous { get; set; }
    public string Next { get; set; }
    public string Last { get; set; }

    public PaginationLinks(string baseUrl, int currentPage, int totalPages, int itemsPerPage)
    {
        First = $"{baseUrl}?page=1&limit={itemsPerPage}";
        Previous = (currentPage > 1 ? $"{baseUrl}?page={currentPage - 1}&limit={itemsPerPage}" : null)!;
        Next = (currentPage < totalPages ? $"{baseUrl}?page={currentPage + 1}&limit={itemsPerPage}" : null)!;
        Last = $"{baseUrl}?page={totalPages}&limit={itemsPerPage}";
    } 
}