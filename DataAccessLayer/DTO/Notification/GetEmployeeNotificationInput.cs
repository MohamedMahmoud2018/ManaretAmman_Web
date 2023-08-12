using DataAccessLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.Notification
{
    public class GetEmployeeNotificationInput:IMustHaveProject
    {
        public int ProjectID { get; set; }
        public int UserId { get; set; }
        public DateTime? Fromdate { get; set; }
        public DateTime? ToDate { get; set; }
       
    }
}
