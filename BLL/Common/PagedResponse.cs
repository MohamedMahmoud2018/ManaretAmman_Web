namespace BusinessLogicLayer.Common;
public class PagedResponse<T>
{
    public int PageIndex { get; set; }
    public int Offset { get; set; }

    public int TotalPages { get; set; }

    public List<T> Data { get; set; }

    public PagedResponse(List<T> data, int pageIndex, int offset)
    {
        this.PageIndex = pageIndex;
        this.Offset    = offset;
        this.Data      = data;
    }
}
