using BusinessLogicLayer.Common;

namespace BusinessLogicLayer.Extensions;
public static class PaginationHelper
{
    public static PagedResponse<T> CreatePagedReponse<T>(this List<T> pagedData, int PageIndex, int Offset, int totalRecords)
    {
        var respose = new PagedResponse<T>(pagedData, PageIndex, Offset);

        var totalPages = ((double)totalRecords / (double)Offset);

        int roundedTotalPages = Convert.ToInt32(Math.Ceiling(totalPages));
       
        respose.TotalPages = roundedTotalPages;

        return respose;
    }
}
