using BusinessLogicLayer.Common;
using BusinessLogicLayer.Extensions;
using BusinessLogicLayer.Services.Auth;
using BusinessLogicLayer.Services.Lookups;
using BusinessLogicLayer.Services.ProjectProvider;
using DataAccessLayer.DTO.Notification;
using DataAccessLayer.Models;

namespace BusinessLogicLayer.Services.Notification
{
    public class NotificationsService : INotificationsService
    {
        private IProjectProvider _projectProvider;
        private readonly ILookupsService _lookupsService;
        private readonly PayrolLogOnlyContext _payrolLogOnlyContext;
        readonly IAuthService _authService;
        readonly int _userId;
        readonly int _projectId;

        public NotificationsService(IProjectProvider projectProvider, ILookupsService lookupsService, PayrolLogOnlyContext payrolLogOnlyContext,  IAuthService authService)
        {
            _projectProvider = projectProvider;
            _lookupsService = lookupsService;
            _payrolLogOnlyContext = payrolLogOnlyContext;
            _authService = authService;
            _userId = _projectProvider.UserId();
            _projectId = _projectProvider.GetProjectId();
        }

        public async Task<int?> AcceptOrRejectNotificationsAsync(AcceptOrRejectNotifcationInput model)
        {
            if( model.CreatedBy ==0) model.CreatedBy=_userId;
            //int? pError = null;
            var result = await _payrolLogOnlyContext.GetProcedures()
                .ChangeEmployeeRequestStatusAsync(model.EmployeeId, model.CreatedBy, model.ApprovalStatusId, model.ApprovalPageID, _projectId, model.Id, model.PrevilageType, 0, null,true, null,null);
            Console.WriteLine(result);
           // OutputParameter<int?> pErrorr = new OutputParameter<int?>(pError);
            return result;
        }

        public async Task<PagedResponse<RemiderOutput>> GetNotificationsAsync(PaginationFilter<GetEmployeeNotificationInput> filter)
        {

            var result = await _payrolLogOnlyContext.GetProcedures()
                        .GetRemindersAsync(_projectId, filter.FilterCriteria.EmployeeID, 1, filter.FilterCriteria.LanguageId, filter.FilterCriteria.Fromdate.DateToIntValue(),
                        filter.FilterCriteria.ToDate.DateToIntValue(), null, _userId , null);

            var totalRecords = result.Count;

            var approvals = await _lookupsService.GetLookups(Constants.Approvals, string.Empty);

            var returnedData = result.Skip((filter.PageIndex - 1) * filter.Offset)
                              .Take(filter.Offset).Select(item => new RemiderOutput
                              {
                                  ID = item.ID,
                                  Date = item.Date,
                                  EmployeeID = item.EmployeeID,
                                  PK = item.PK,
                                  Notes = item.Notes,
                                  ApprovalStatusID = item.ApprovalStatusID,
                                  StatusID = item.StatusID,
                                  PrivillgeType = item.PrivillgeType,
                                  TypeID = item.TypeID,
                                  StatusDesc = item.StatusDesc,
                                  ApprovalProcessID = item.ApprovalProcessID,
                                  NextApprovalID = item.NextApprovalID,
                                  AllowAccept = item.AllowAccept,
                                  AllowDelete = item.AllowDelete,
                                  AllowEdit = item.AllowEdit,
                                  AllowReject = item.AllowReject
                              }).ToList();

            return returnedData.CreatePagedReponse(filter.PageIndex, filter.Offset, totalRecords);

        }
    }
}
