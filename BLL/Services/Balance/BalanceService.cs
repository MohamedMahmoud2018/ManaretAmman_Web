using AutoMapper;
using BusinessLogicLayer.UnitOfWork;
using DataAccessLayer.DTO;
using DataAccessLayer.Models;

namespace BusinessLogicLayer.Services.Balance
{
    public class BalanceService : IBalanceService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;
        private readonly PayrolLogOnlyContext _payrolLogOnlyContext;
        public BalanceService(IUnitOfWork unit, IMapper mapper, PayrolLogOnlyContext payrolLogOnlyContext)
        {
            _unit = unit;
            _mapper = mapper;
            _payrolLogOnlyContext= payrolLogOnlyContext;
        }

        public async Task<List<GetEmployeeBalanceReportResult>> Get(EmployeeBalancesInput balanceData)
        {
            var result=await _payrolLogOnlyContext.GetProcedures().GetEmployeeBalanceReportAsync(balanceData.EmployeeID, balanceData.YearID, balanceData.ProjectID, 1, 0,null,null,null);
            
            return result;
        }
    }
}
