using BusinessLogicLayer.Common;
using BusinessLogicLayer.Services.EmployeeAttendance;
using BusinessLogicLayer.Services.Notification;
using DataAccessLayer.DTO.EmployeeAttendance;
using DataAccessLayer.DTO.Notification;
using DataAccessLayer.Models;
using ManaretAmman.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ManaretAmman.Controllers.EmployeeMonitor
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeAttendanceController : ControllerBase
    {
        private readonly IEmployeeAttendanceService _employeeAttendanceService;

        public EmployeeAttendanceController(IEmployeeAttendanceService employeeAttendanceService)
        => _employeeAttendanceService = employeeAttendanceService;

        [HttpGet("GetPage")]
        public async Task<IApiResponse> GetPage([FromQuery] PaginationFilter<EmployeeAttendanceInput> filter)
        {
            var result = await _employeeAttendanceService.GetEmployeeAttendance(filter);

            if (result == null)
                return ApiResponse<BusinessLogicLayer.Common.PagedResponse<EmployeeAttendanceOutput>>.Failure(default, null);
            if (result.Result.Count == 0)
                return ApiResponse<BusinessLogicLayer.Common.PagedResponse<EmployeeAttendanceOutput>>.Success("No data", result);


            return ApiResponse<BusinessLogicLayer.Common.PagedResponse<EmployeeAttendanceOutput>>.Success("data has been retrieved succussfully", result);
        }
    }
}
