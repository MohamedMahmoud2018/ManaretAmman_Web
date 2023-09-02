namespace ManaretAmman.Models.Pagination;

public class PaginationFilter
{
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public PaginationFilter()
    {
        this.PageIndex = 1;
        this.PageSize = 10;
    }
    public PaginationFilter(int pageNumber, int pageSize)
    {
        this.PageIndex = pageNumber < 1 ? 1 : pageNumber;
        this.PageSize  = pageSize > 10 ? 10 : pageSize;
    }
}
