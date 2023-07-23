using BusinessLogicLayer.Services.Balance;
using DataAccessLayer.DTO;
using DataAccessLayer.Models;
using ManaretAmman.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ManaretAmman.Controllers.Employees
{
    [Route("api/Employees/[controller]")]
    [ApiController]
    public class BalanceController : ControllerBase
    {
        private IBalanceService balanceService;

        public BalanceController(IBalanceService balanceService)
        {
            this.balanceService = balanceService;
        }
        [HttpPost("Get")]
        public async Task<IApiResponse> Get(EmployeeBalancesInput balanceData)
        {
            var result =await balanceService.Get(balanceData);
            return ApiResponse<List<GetEmployeeBalanceReportResult>>.Success(result);
        }
        }
}
