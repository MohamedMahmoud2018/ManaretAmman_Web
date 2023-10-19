using AutoMapper;
using BusinessLogicLayer.Services.Auth;
using BusinessLogicLayer.Services.Notification;
using BusinessLogicLayer.Services.ProjectProvider;
using BusinessLogicLayer.UnitOfWork;
using DataAccessLayer.DTO;
using DataAccessLayer.Models;

namespace BusinessLogicLayer.Services.Balance
{
    public class BalanceService : IBalanceService
    {
        
        private readonly PayrolLogOnlyContext _payrolLogOnlyContext;
        readonly IProjectProvider _projectProvider;
        readonly IAuthService _authService;
        readonly int _userId;
        readonly int _projecId;
        public BalanceService( PayrolLogOnlyContext payrolLogOnlyContext, IProjectProvider projectProvider, IAuthService authService )
        {
        
            _payrolLogOnlyContext= payrolLogOnlyContext;
            _projectProvider = projectProvider;
            _authService = authService;
            _userId = _projectProvider.UserId();
            _projecId = _projectProvider.GetProjectId();
        }

        public async Task<List<GetEmployeeBalanceReportResult>> Get(EmployeeBalancesInput balanceData)
        {
            if (_userId == -1) throw new UnauthorizedAccessException("Incorrect userId");
            if (!_authService.IsValidUser(_userId)) throw new UnauthorizedAccessException("Incorrect userId");

            var result=await _payrolLogOnlyContext.GetProcedures().GetEmployeeBalanceReportAsync(balanceData.EmployeeID, balanceData.YearID, balanceData.ProjectID, 1, 0,null,null,null);
            
            return result;
        }
    }
}
