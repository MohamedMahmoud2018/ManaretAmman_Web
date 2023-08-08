namespace BusinessLogicLayer.Common;

public class PaginationFilter
{
    private int _pageIndex = 1;
    private int _offset    = 10;

    public int PageIndex
    {
        get => _pageIndex;
        set => _pageIndex = value < 1 ? 1 : value;
    }

    public int Offset
    {
        get => _offset;
        set => _offset = value > 10 ? 10 : value;
    }

    public string Search { get; set; }

    public PaginationFilter()
    {
    }
}

