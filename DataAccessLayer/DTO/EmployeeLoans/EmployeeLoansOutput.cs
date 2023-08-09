using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.DTO
{
    public class EmployeeLoansOutput
    {
        public EmployeeLoansOutput() { }
        public int ID { get; set; }
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public int? CreatedBy { get; set; }
        public string CreatedName { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? ModifiedBy { get; set; }
        public string ModifiedName { get; set; }
        public DateTime? ModificationDate { get; set; }
        public int ProjectID { get; set; }
        public DateTime? LoanDate { get; set; }

        [Column(TypeName = "decimal(18, 5)")]
        public decimal? LoanAmount { get; set; }
        [StringLength(200)]
        public string Notes { get; set; }
        public int? LoantypeId { get; set; }
        public string? loantypeEn { get; set; }
        public string? loantypeAr { get; set; }
        public string ApprovalStatus { get; set; }
        public int? LoanSerial { get; set; }
    }
}
