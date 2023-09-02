using BusinessLogicLayer.Common;
using BusinessLogicLayer.Services.EmployeeLoans;
using DataAccessLayer.DTO;
using DataAccessLayer.DTO.EmployeeLoans;
using ManaretAmman.Models;
using Microsoft.AspNetCore.Mvc;

namespace ManaretAmman.Controllers.Employees
{
    [Route("api/Employees/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private readonly IEmployeeLoansService _employeeService;

        public LoansController(IEmployeeLoansService employeeService)
        => _employeeService = employeeService;

        [HttpGet("GetPage")]
        public async Task<IApiResponse> GetPage([FromQuery] PaginationFilter<EmployeeLoanFilter> filter)
        {
            var result = await  _employeeService.GetPage(filter);

            return ApiResponse<BusinessLogicLayer.Common.PagedResponse<EmployeeLoansOutput>>.Success("data has been retrieved succussfully", result);
        }

        [HttpGet]
        public async Task<IApiResponse> Get(int id)
        {
            var result = await _employeeService.Get(id);

            return ApiResponse<EmployeeLoansOutput>.Success("data has been retrieved succussfully", result);
        }

        [HttpPost]
        public async Task<IApiResponse> Create(EmployeeLoansInput employee)
        {
            await _employeeService.Create(employee);

            return ApiResponse.Success();
        }

        [HttpPut]
        public async Task<IApiResponse> Update(EmployeeLoansUpdate employee)
        {
            await _employeeService.Update(employee);

            return ApiResponse.Success();
        }


        [HttpDelete]
        public async Task<IApiResponse> Delete(int employeeLoanId)
        {
            await _employeeService.Delete(employeeLoanId);
            return ApiResponse.Success();
        }
    }

}

