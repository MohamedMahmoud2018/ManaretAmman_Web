using BusinessLogicLayer.Services.Employees;
using DataAccessLayer.DTO.Employees;
using ManaretAmman.Models;
using Microsoft.AspNetCore.Mvc;

namespace ManaretAmman.Controllers.Employees
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet("GetList")]
        public async Task<IApiResponse> GetList() 
        {
            var result = await _employeeService.GetList();

            return ApiResponse<List<EmployeeLookup>>.Success("data has been retrieved succussfully", result);
        }
    }
}
