﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Models
{
    [Table("EmployeeLoan")]
    public partial class EmployeeLoan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmployeeLoanID { get; set; }
        [Key]
        public int EmployeeID { get; set; }
        public int? LoanDate { get; set; }
        [Column(TypeName = "decimal(18, 5)")]
        public decimal? LoanAmount { get; set; }
        [StringLength(200)]
        [Unicode(false)]
        public string Notes { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreationDate { get; set; }
        public int? ModifiedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModificationDate { get; set; }
        [Key]
        public int ProjectID { get; set; }
        public int? loantypeid { get; set; }
        public int? StatusID { get; set; }
        public int? ApprovalStatusID { get; set; }
        public int? LoanSerial { get; set; }

        [ForeignKey("EmployeeID")]
        [InverseProperty("EmployeeLoans")]
        public virtual Employee Employee { get; set; }
        [ForeignKey("ProjectID")]
        [InverseProperty("EmployeeLoans")]
        public virtual Project Project { get; set; }
    }
}