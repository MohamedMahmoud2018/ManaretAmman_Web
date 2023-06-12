﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Models
{
    [Index("EndDate", Name = "IX_EmployeeContract11")]
    [Index("EndDate", Name = "IX_EmployeeContractEndDate_IX1")]
    [Index("EmployeeID", Name = "ix_EmployeeContract_1")]
    [Index("StartDate", Name = "ix_EmployeeContract_2")]
    public partial class EmployeeContract
    {
        [Key]
        public int ContractID { get; set; }
        public int? StartDate { get; set; }
        public int? EndDate { get; set; }
        public int? EmployeeID { get; set; }
        [Column(TypeName = "decimal(18, 5)")]
        public decimal? Salary { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreationDate { get; set; }
        public int? ModifiedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModificationDate { get; set; }
        [Column(TypeName = "decimal(18, 5)")]
        public decimal? SocialSecuritySalary { get; set; }
        public int ProjectID { get; set; }
        public int? IsDailyWork { get; set; }
        public int? ContractTypeID { get; set; }
        public int? ConfirmationDate { get; set; }
        public int? CancelDate { get; set; }
        public int? StatusID { get; set; }
        public int? incometaxtype { get; set; }
        public int? CompanyID { get; set; }

        [ForeignKey("EmployeeID")]
        [InverseProperty("EmployeeContracts")]
        public virtual Employee Employee { get; set; }
        [ForeignKey("ProjectID")]
        [InverseProperty("EmployeeContracts")]
        public virtual Project Project { get; set; }
    }
}