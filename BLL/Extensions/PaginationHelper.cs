using BusinessLogicLayer.Common;

namespace BusinessLogicLayer.Extensions;
public static class PaginationHelper
{
    public static PagedResponse<T> CreatePagedReponse<T>(this List<T> pagedData, PaginationFilter filter, int totalRecords)
    {
        var respose = new PagedResponse<T>(pagedData, filter.PageIndex, filter.Offset);

        var totalPages = ((double)totalRecords / (double)filter.Offset);

        int roundedTotalPages = Convert.ToInt32(Math.Ceiling(totalPages));
       
        respose.TotalPages = roundedTotalPages;

        return respose;
    }
}
