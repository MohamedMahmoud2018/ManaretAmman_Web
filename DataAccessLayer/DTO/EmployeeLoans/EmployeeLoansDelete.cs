using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO
{
    public class EmployeeLoansDelete
    {
        public int EmployeeLeaveID { get; set; }
        public int EmployeeID { get; set; }
        public int ProjectID { get; set; }
    }
}
