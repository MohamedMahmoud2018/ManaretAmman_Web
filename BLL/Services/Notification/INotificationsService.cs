using DataAccessLayer.DTO.Notification;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services.Notification
{
    public interface INotificationsService
    {
        public Task<List<GetRemindersResult>> GetNotificationsAsync(GetEmployeeNotificationInput model);
       // public Task<List<GetRemindersResult>> IgnorNotificationsAsync(GetEmployeeNotificationInput model);
        public Task<List<ChangeEmployeeRequestStatusResult>> AcceptOrRejectNotificationsAsync(AcceptOrRejectNotifcationInput model);




    }
}
