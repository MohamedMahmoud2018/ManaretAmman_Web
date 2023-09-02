using DataAccessLayer.DTO.Employees;

namespace BusinessLogicLayer.Services.Employees;

public interface IEmployeeService
{
    Task<List<EmployeeLookup>> GetList();
}
