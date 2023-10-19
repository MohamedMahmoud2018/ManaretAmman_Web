using BusinessLogicLayer.Common;
using DataAccessLayer.DTO.Notification;
using DataAccessLayer.Models;

namespace BusinessLogicLayer.Services.Notification
{
    public interface INotificationsService
    {
        public Task<PagedResponse<RemiderOutput>> GetNotificationsAsync(PaginationFilter<GetEmployeeNotificationInput> filter);
       // public Task<List<GetRemindersResult>> IgnorNotificationsAsync(GetEmployeeNotificationInput model);
        public Task<int?> AcceptOrRejectNotificationsAsync(AcceptOrRejectNotifcationInput model);




    }
}
