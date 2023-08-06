using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.EmployeeLoans
{
    public class EmployeeLoansUpdate
    {
        public int ID { get; set; }
        public int EmployeeID { get; set; }
        public DateTime? LoanDate { get; set; }

        [Column(TypeName = "decimal(18, 5)")]
        public decimal? LoanAmount { get; set; }

        [StringLength(200)]
        public string? Notes { get; set; }
        public int? LoantypeId { get; set; }

        public int? LoanSerial { get; set; }
    }
}
