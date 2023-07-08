using BusinessLogicLayer.Services.EmployeeLeaves;
using DataAccessLayer.DTO;
using ManaretAmman.Models;
using Microsoft.AspNetCore.Mvc;

namespace ManaretAmman.Controllers.Employees
{
    [Route("api/Employees/[controller]")]
    [ApiController]
    public class LeavesController : ControllerBase
    {
        private readonly IEmployeeLeavesService _employeeService;

        public LeavesController(IEmployeeLeavesService employeeService)
        => _employeeService = employeeService;

        [HttpGet("GetAll")]
        public async Task<IApiResponse> GetAll()
        {
            var result = await  _employeeService.GetAll();

            return ApiResponse<List<EmployeeLeavesOutput>>.Success("data has been retrieved succussfully", result);
        }

        [HttpGet]
        public async Task<IApiResponse> Get(int id)
        {
            var result = await _employeeService.Get(id);

            return ApiResponse<EmployeeLeavesOutput>.Success("data has been retrieved succussfully", result);
        }

        [HttpPost]
        public async Task<IApiResponse> Create(EmployeeLeavesInput employee)
        {
            await _employeeService.Create(employee);

            return ApiResponse.Success();
        }

        [HttpPut]
        public async Task<IApiResponse> Update(EmployeeLeavesInput employee)
        {
            await _employeeService.Update(employee);

            return ApiResponse.Success();
        }


        [HttpDelete]
        public async Task<IApiResponse> Delete(int employeeLeaveId)
        {
            await _employeeService.Delete(employeeLeaveId);
            return ApiResponse.Success();
        }
    }

}

