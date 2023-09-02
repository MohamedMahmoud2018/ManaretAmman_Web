using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.EmployeeVacations
{
    public class EmployeeVacationsUpdate
    {
        public int ID { get; set; }
        public int EmployeeID { get; set; }
        public int? VacationTypeID { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        [StringLength(1000)]
        public string? Notes { get; set; }
    }
}
