﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Models
{
    public partial class EmployeeDeduction
    {
        [Key]
        public int EmployeeDeductionID { get; set; }
        public int? EmployeeID { get; set; }
        public int? StartDate { get; set; }
        public int? EndDate { get; set; }
        public int? TypeID { get; set; }
        [Column(TypeName = "decimal(18, 5)")]
        public decimal? Amount { get; set; }
        public int? AllowanceID { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreationDate { get; set; }
        public int? ModifiedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModificationDate { get; set; }
        [Key]
        public int ProjectID { get; set; }

        [ForeignKey("EmployeeID")]
        [InverseProperty("EmployeeDeductions")]
        public virtual Employee Employee { get; set; }
        [ForeignKey("ProjectID")]
        [InverseProperty("EmployeeDeductions")]
        public virtual Project Project { get; set; }
    }
}