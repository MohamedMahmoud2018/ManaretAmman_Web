using BusinessLogicLayer.Common;
using BusinessLogicLayer.Services.EmployeeVacations;
using DataAccessLayer.DTO;
using DataAccessLayer.DTO.EmployeeVacations;
using ManaretAmman.Models;
using Microsoft.AspNetCore.Mvc;

namespace ManaretAmman.Controllers.Employees;

[Route("api/Employees/[controller]")]
[ApiController]
public class VacationsController : ControllerBase
{
    private readonly IEmployeeVacationService _employeeService;

    public VacationsController(IEmployeeVacationService employeeService)
    => _employeeService = employeeService;

    [HttpGet("GetPage")]
    public async Task<IApiResponse> GetPage([FromQuery] PaginationFilter<EmployeeVacationFilter> filter)
    {
        var result = await  _employeeService.GetPage(filter);

        return ApiResponse<BusinessLogicLayer.Common.PagedResponse<EmployeeVacationOutput>>.Success("data has been retrieved succussfully", result);
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
    public async Task<IApiResponse> Update(EmployeeVacationsUpdate employee)
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

