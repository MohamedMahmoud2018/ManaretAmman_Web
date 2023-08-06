namespace ManaretAmman.Models;

public class PagedResponse<T> : ApiResponse<T>
{
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int TotalRecords { get; set; }

    public PagedResponse(T data, int pageNumber, int pageSize) : base(true, null, null)
    {
        this.PageIndex = pageNumber;
        this.PageSize   = pageSize;
        this.Data       = data;
    }
}