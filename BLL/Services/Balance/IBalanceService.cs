using DataAccessLayer.DTO;
using DataAccessLayer.Models;

namespace BusinessLogicLayer.Services.Balance
{
    public interface IBalanceService
    {
        Task<List<GetEmployeeBalanceReportResult>> Get(EmployeeBalancesInput balanceData);
       
    }
}
