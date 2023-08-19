using AutoMapper;
using BusinessLogicLayer.Common;
using BusinessLogicLayer.Extensions;
using BusinessLogicLayer.Services.ProjectProvider;
using BusinessLogicLayer.UnitOfWork;
using DataAccessLayer.DTO.Notification;
using DataAccessLayer.Models;

namespace BusinessLogicLayer.Services.Notification
{
    public class NotificationsService : INotificationsService
    {
        private IProjectProvider _projectProvider;
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;
        private readonly PayrolLogOnlyContext _payrolLogOnlyContext;
        public NotificationsService(IProjectProvider projectProvider, IUnitOfWork unit, IMapper mapper, PayrolLogOnlyContext payrolLogOnlyContext)
        {
            _projectProvider      = projectProvider;
            _unit                 = unit;
            _mapper               = mapper;
            _payrolLogOnlyContext = payrolLogOnlyContext;
        }

        public async Task<List<ChangeEmployeeRequestStatusResult>> AcceptOrRejectNotificationsAsync(AcceptOrRejectNotifcationInput model)
        {
            
            var result = await _payrolLogOnlyContext.GetProcedures().ChangeEmployeeRequestStatusAsync(model.EmoloyeeId, model.CreatedBy, model.ApprovalStatusId, model.ApprovalPageID, model.ProjectID, model.Id, model.PrevilageType, 0,null,null);
            return result;
        }

        public Task<PagedResponse<RemiderOutput>> GetNotificationsAsync(PaginationFilter filter)
        {
            var projectId = _projectProvider.GetProjectId();

            var result = await _payrolLogOnlyContext.GetProcedures().GetRemindersAsync(projectId, null, 1, 0, filter..Fromdate.DateToIntValue(), model.ToDate.DateToIntValue(), null, model.UserId, null);
        }

        //public async Task<List<RemiderOutput>> GetNotificationsAsync(GetEmployeeNotificationInput model)
        //{
        //    var result = await _payrolLogOnlyContext.GetProcedures().GetRemindersAsync(model.ProjectID, null, 1, 0, model.Fromdate.DateToIntValue(), model.ToDate.DateToIntValue(), null, model.UserId, null);
        //    return _mapper.Map<List<RemiderOutput>>(result);
        //}
    }
}
