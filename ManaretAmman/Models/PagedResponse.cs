namespace ManaretAmman.Models;

public class PagedResponse<T> 
{
    public int PageIndex { get; set; }
    public int Offset { get; set; }

    public int TotalPages { get; set; }
    public int TotalRecords { get; set; }

    public T Data { get; set; }

    public PagedResponse(T data, int pageIndex, int offset)
    {
        this.PageIndex = pageIndex;
        this.Offset    = offset;
        this.Data      = data;
    }
}
