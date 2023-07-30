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
        public Task<List<GetRemindersResult>> GetRemindersAsync(int projectId,int userId);


    }
}
