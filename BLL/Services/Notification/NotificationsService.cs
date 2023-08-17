using AutoMapper;
using BusinessLogicLayer.Extensions;
using BusinessLogicLayer.UnitOfWork;
using DataAccessLayer.DTO.Notification;
using DataAccessLayer.Models;

namespace BusinessLogicLayer.Services.Notification
{
    public class NotificationsService : INotificationsService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;
        private readonly PayrolLogOnlyContext _payrolLogOnlyContext;
        public NotificationsService(IUnitOfWork unit, IMapper mapper, PayrolLogOnlyContext payrolLogOnlyContext)
        {
            _unit = unit;
            _mapper = mapper;
            _payrolLogOnlyContext = payrolLogOnlyContext;
        }

        public async Task<List<ChangeEmployeeRequestStatusResult>> AcceptOrRejectNotificationsAsync(AcceptOrRejectNotifcationInput model)
        {
            
            var result = await _payrolLogOnlyContext.GetProcedures().ChangeEmployeeRequestStatusAsync(model.EmoloyeeId, model.CreatedBy, model.ApprovalStatusId, model.ApprovalPageID, model.ProjectID, model.Id, model.PrevilageType, 0,null,null);
            return result;
        }

        public async Task<List<RemiderOutput>> GetNotificationsAsync(GetEmployeeNotificationInput model)
        {
            var result = await _payrolLogOnlyContext.GetProcedures().GetRemindersAsync(model.ProjectID, null, 1, 0, model.Fromdate.DateToIntValue(), model.ToDate.DateToIntValue(), null, model.UserId, null);
            return _mapper.Map<List<RemiderOutput>>(result);
        }
    }
}
