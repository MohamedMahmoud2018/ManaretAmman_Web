using DataAccessLayer.DTO;
using System.Net;

namespace BusinessLogicLayer.Services.EmployeeLeaves
{
    public interface IEmployeeLeavesService
    {
        Task Create(EmployeeLeavesInput employee);
        Task Update(EmployeeLeavesInput employee);
        Task Delete(int employeeLeaveId);
        Task<EmployeeLeavesOutput> Get(int id);
        Task<List<EmployeeLeavesOutput>> GetAll();
    }
}
