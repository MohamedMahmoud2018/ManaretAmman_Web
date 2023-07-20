using DataAccessLayer.DTO;

namespace BusinessLogicLayer.Services.EmployeeLoans
{
    public interface IEmployeeLoansService
    {
        Task Create(EmployeeLoansInput employee);
        Task Update(EmployeeLoansInput employee);
        Task Delete(int employeeLoanId);
        Task<EmployeeLoansOutput> Get(int id);
        Task<List<EmployeeLoansOutput>> GetAll();
    }
}
