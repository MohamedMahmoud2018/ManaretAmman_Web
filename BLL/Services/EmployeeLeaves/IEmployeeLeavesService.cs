using BusinessLogicLayer.Common;
using DataAccessLayer.DTO;
using DataAccessLayer.DTO.EmployeeLeaves;

namespace BusinessLogicLayer.Services.EmployeeLeaves;

public interface IEmployeeLeavesService
{
    Task Create(EmployeeLeavesInput employee);
    Task Update(EmployeeLeavesUpdate employee);
    Task Delete(int employeeLeaveId);
    Task<EmployeeLeavesOutput> Get(int id);
    Task<PagedResponse<EmployeeLeavesOutput>> GetPage(PaginationFilter<EmployeeLeaveFilter> filter);
}
