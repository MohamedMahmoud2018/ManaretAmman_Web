using AutoMapper;
using BusinessLogicLayer.Common;
using BusinessLogicLayer.Extensions;
using BusinessLogicLayer.Services.Lookups;
using BusinessLogicLayer.Services.ProjectProvider;
using BusinessLogicLayer.UnitOfWork;
using DataAccessLayer.DTO.Notification;
using DataAccessLayer.Models;

namespace BusinessLogicLayer.Services.Notification
{
    public class NotificationsService : INotificationsService
    {
        private IProjectProvider _projectProvider;
        private readonly ILookupsService _lookupsService;
        private readonly PayrolLogOnlyContext _payrolLogOnlyContext;
        public NotificationsService(IProjectProvider projectProvider, ILookupsService lookupsService, PayrolLogOnlyContext payrolLogOnlyContext)
        {
            _projectProvider = projectProvider;
            _lookupsService = lookupsService;
            _payrolLogOnlyContext = payrolLogOnlyContext;
        }

        public async Task<List<ChangeEmployeeRequestStatusResult>> AcceptOrRejectNotificationsAsync(AcceptOrRejectNotifcationInput model)
        {
            var projectId = _projectProvider.GetProjectId();
            var result = await _payrolLogOnlyContext.GetProcedures()
                .ChangeEmployeeRequestStatusAsync(model.EmoloyeeId, model.CreatedBy, model.ApprovalStatusId, model.ApprovalPageID, projectId, model.Id, model.PrevilageType, 0, null, null);
            return result;
        }

        public async Task<PagedResponse<RemiderOutput>> GetNotificationsAsync(PaginationFilter<GetEmployeeNotificationInput> filter)
        {
            var projectId = _projectProvider.GetProjectId();

            var result = await _payrolLogOnlyContext.GetProcedures()
                        .GetRemindersAsync(projectId, null, 1, 0, filter.FilterCriteria.Fromdate.DateToIntValue(),
                        filter.FilterCriteria.ToDate.DateToIntValue(), null, filter.FilterCriteria.UserId, null);

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
                                  ApprovalStatus = approvals.FirstOrDefault(e => e.ID == item.ApprovalStatusID)?.ColumnDescription,
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
