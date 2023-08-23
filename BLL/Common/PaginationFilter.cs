namespace BusinessLogicLayer.Common;

public class PaginationFilter<T>
{
    private int _pageIndex = 1;
    private int _offset    = 10;

    public T FilterCriteria { get; set; }

    public int PageIndex
    {
        get => _pageIndex;
        set => _pageIndex = value < 1 ? 1 : value;
    }

    public int Offset
    {
        get => _offset;
        set => _offset = value <= 0 ? 10 : value;
    }
}

