﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.DTO
{
    public class EmployeeLoansInput
    {
        public int ID { get; set; }
        public int EmployeeID { get; set; }
        //public int? CreatedBy { get; set; }
        //public DateTime? CreationDate { get; set; }
        //public int? ModifiedBy { get; set; }
        //public DateTime? ModificationDate { get; set; }
        public int ProjectID { get; set; }
        public DateTime? LoanDate { get; set; }
        [Column(TypeName = "decimal(18, 5)")]
        public decimal? LoanAmount { get; set; }
        [StringLength(200)]
        public string Notes { get; set; }
        public int? loantypeid { get; set; }
        //public int? StatusID { get; set; }
        //public int? ApprovalStatusID { get; set; }
        public int? LoanSerial { get; set; }
    }
}
