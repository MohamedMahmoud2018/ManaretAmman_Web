using BusinessLogicLayer.Common;
using BusinessLogicLayer.Extensions;
using BusinessLogicLayer.Services.Auth;
using BusinessLogicLayer.Services.Lookups;
using BusinessLogicLayer.Services.ProjectProvider;
using DataAccessLayer.DTO.EmployeeAttendance;
using DataAccessLayer.Models;

namespace BusinessLogicLayer.Services.EmployeeAttendance
{
    public class EmployeeAttendanceService: IEmployeeAttendanceService
    {
        private IProjectProvider _projectProvider;
        private readonly ILookupsService _lookupsService;
        private readonly PayrolLogOnlyContext _payrolLogOnlyContext;
        readonly IAuthService _authService;
        readonly int _userId;
        readonly int _projectId;
        public EmployeeAttendanceService(IProjectProvider projectProvider, ILookupsService lookupsService, PayrolLogOnlyContext payrolLogOnlyContext, IAuthService authService) 
        {
            _projectProvider = projectProvider;
            _lookupsService = lookupsService;
            _payrolLogOnlyContext = payrolLogOnlyContext;
            _authService = authService;
            _userId = _projectProvider.UserId();
            _projectId = _projectProvider.GetProjectId();
        }

        public async Task<PagedResponse<EmployeeAttendanceOutput>> GetEmployeeAttendance(PaginationFilter<EmployeeAttendanceInput> filter)
        {
            var _filter = filter.FilterCriteria;
            var result = await _payrolLogOnlyContext.GetProcedures().GetEmployeeAttendanceAsync(_filter.EmployeeID, _filter.FromDate.DateToIntValue(), _filter.ToDate.DateToIntValue(), _projectId, _filter.YearId, null, _filter.Flag, null, _filter.LanguageID, null);

            var totalRecords = result.Count;

            var approvals = await _lookupsService.GetLookups(Constants.Approvals, string.Empty);
            var returnedData = result.Skip((filter.PageIndex - 1) * filter.Offset)
                              .Take(filter.Offset).Select(item => new EmployeeAttendanceOutput
                              {
                                  EmployeeName = item.EmployeeName,
                                  AttendanceDate = item.AttendanceDate.IntToDateValue(),
                                  DayDesc = item.DayDesc,
                                  EmployeeNumber = item.EmployeeNumber,
                                  EndTime = item.EndTime.ConvertFromMinutesToTimeString(),
                                  Notes = item.Notes,
                                  ShiftName = item.ShiftName,
                                  StartTime = item.StartTime.ConvertFromMinutesToTimeString(),
                                  Workhours= (TimeSpan.FromMinutes((double)item.EndTime)- TimeSpan.FromMinutes((double)item.StartTime)).ToString(@"hh\:mm"),
                                  ShiftWithTimes = $"{item.StartTime.ConvertFromMinutesToTimeString()} | {item.EndTime.ConvertFromMinutesToTimeString()} | {item.ShiftName}"
                              }).ToList();

            return returnedData.CreatePagedReponse<EmployeeAttendanceOutput>(filter.PageIndex, filter.Offset, totalRecords);
        }
        
    }
}
