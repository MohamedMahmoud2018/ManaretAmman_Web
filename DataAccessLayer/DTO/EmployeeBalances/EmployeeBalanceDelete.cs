using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO
{
    public class EmployeeBalanceDelete
    {
        public int EmployeeBalanceID { get; set; }
        public int EmployeeID { get; set; }
        public int ProjectID { get; set; }
    }
}
