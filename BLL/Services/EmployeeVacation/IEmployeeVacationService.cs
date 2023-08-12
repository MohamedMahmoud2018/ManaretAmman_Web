using BusinessLogicLayer.Common;
using DataAccessLayer.DTO;
using DataAccessLayer.DTO.EmployeeVacations;

namespace BusinessLogicLayer.Services.EmployeeVacations
{
    public interface IEmployeeVacationService
    {
        Task Create(EmployeeVacationInput employeeVacation);
        Task Update(EmployeeVacationsUpdate employeeVacation);
        Task Delete(int employeeVacationId);
        Task<EmployeeVacationOutput> Get(int id);
        Task<PagedResponse<EmployeeVacationOutput>> GetPage(PaginationFilter<EmployeeVacationFilter> filter);
    }
}
