using BusinessLogicLayer.Common;
using DataAccessLayer.DTO;
using DataAccessLayer.DTO.EmployeeLoans;

namespace BusinessLogicLayer.Services.EmployeeLoans
{
    public interface IEmployeeLoansService
    {
        Task Create(EmployeeLoansInput employee);
        Task Update(EmployeeLoansUpdate employee);
        Task Delete(int employeeLoanId);
        Task<EmployeeLoansOutput> Get(int id);
        Task<PagedResponse<EmployeeLoansOutput>> GetPage(PaginationFilter<EmployeeLoanFilter> filter);
    }
}
