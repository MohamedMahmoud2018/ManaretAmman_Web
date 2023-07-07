using DataAccessLayer.DTO;
using DataAccessLayer.Models;

namespace BusinessLogicLayer.Services.Lookups
{
    public interface ILookupsService
    {
        Task<IList<LookupDto>> GetLookups(string tableName, string columnName);
        Task<string> GetDescription(string tableName, string columnName, int columnValue);
        Task<string> GetDescriptionAr(string tableName, string columnName, int columnValue);
    }
}
