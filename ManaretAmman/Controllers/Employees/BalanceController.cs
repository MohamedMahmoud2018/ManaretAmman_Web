﻿using BusinessLogicLayer.Services.Balance;
using DataAccessLayer.DTO;
using DataAccessLayer.Models;
using ManaretAmman.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

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
            if (result == null || result.Count == 0) {
                List<GetEmployeeBalanceReportResult> res = new List<GetEmployeeBalanceReportResult>();
                res.Add(new GetEmployeeBalanceReportResult());
                return ApiResponse<List<GetEmployeeBalanceReportResult>>.Failure( res,null); }
            return ApiResponse<List<GetEmployeeBalanceReportResult>>.Success(result);
        }
        }
}
