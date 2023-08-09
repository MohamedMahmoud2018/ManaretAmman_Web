using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.Notification
{
    public class GetEmployeeNotificationInput
    {
        public int projectId { get; set; }
        public int userId { get; set; }
        public DateTime? fromdate { get; set; }
        public DateTime? toDate { get; set; }
       
    }
}
