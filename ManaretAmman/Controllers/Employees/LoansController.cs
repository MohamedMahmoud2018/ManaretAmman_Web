using BusinessLogicLayer.Services.EmployeeLoans;
using DataAccessLayer.DTO;
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

        [HttpGet("GetAll")]
        public async Task<IApiResponse> GetAll()
        {
            var result = await  _employeeService.GetAll();

            return ApiResponse<List<EmployeeLoansOutput>>.Success("data has been retrieved succussfully", result);
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
        public async Task<IApiResponse> Update(EmployeeLoansInput employee)
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

