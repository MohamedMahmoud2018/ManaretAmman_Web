using BusinessLogicLayer.Services.EmployeeVacations;
using DataAccessLayer.DTO;
using ManaretAmman.Models;
using Microsoft.AspNetCore.Mvc;

namespace ManaretAmman.Controllers.Employees
{
    [Route("api/Employees/[controller]")]
    [ApiController]
    public class VacationController : ControllerBase
    {
        private readonly IEmployeeVacationService _employeeService;

        public VacationController(IEmployeeVacationService employeeService)
        => _employeeService = employeeService;

        [HttpGet("GetAll")]
        public async Task<IApiResponse> GetAll()
        {
            var result = await  _employeeService.GetAll();

            return ApiResponse<List<EmployeeVacationOutput>>.Success("data has been retrieved succussfully", result);
        }

        [HttpGet]
        public async Task<IApiResponse> Get(int id)
        {
            var result = await _employeeService.Get(id);

            return ApiResponse<EmployeeVacationOutput>.Success("data has been retrieved succussfully", result);
        }

        [HttpPost]
        public async Task<IApiResponse> Create(EmployeeVacationInput employee)
        {
            await _employeeService.Create(employee);

            return ApiResponse.Success();
        }

        [HttpPut]
        public async Task<IApiResponse> Update(EmployeeVacationInput employee)
        {
            await _employeeService.Update(employee);

            return ApiResponse.Success();
        }


        [HttpDelete]
        public async Task<IApiResponse> Delete(int employeeVacationId)
        {
            await _employeeService.Delete(employeeVacationId);
            return ApiResponse.Success();
        }
    }

}

