using DataAccessLayer.DTO;

namespace BusinessLogicLayer.Services.EmployeeVacations
{
    public interface IEmployeeVacationService
    {
        Task Create(EmployeeVacationInput employeeVacation);
        Task Update(EmployeeVacationInput employeeVacation);
        Task Delete(int employeeVacationId);
        Task<EmployeeVacationOutput> Get(int id);
        Task<List<EmployeeVacationOutput>> GetAll();
    }
}
