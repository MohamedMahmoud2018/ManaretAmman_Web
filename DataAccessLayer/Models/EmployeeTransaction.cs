﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Models
{
    [Table("EmployeeTransaction")]
    [Index("TransactionDate", Name = "IX_EmployeeTransaction_1")]
    [Index("EmployeeID", "TransactionDate", Name = "IX_EmployeeTransaction_2")]
    public partial class EmployeeTransaction
    {
        [Key]
        public int EmployeeTransactionID { get; set; }
        [Key]
        public int EmployeeID { get; set; }
        public int? TransactionDate { get; set; }
        public int? TransactionTypeID { get; set; }
        public int? TransactionInMinutes { get; set; }
        [StringLength(100)]
        [Unicode(false)]
        public string Notes { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreationDate { get; set; }
        public int? ModifiedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModificationDate { get; set; }
        public int? BySystem { get; set; }
        public int? RelatedToDate { get; set; }
        [Key]
        public int ProjectID { get; set; }
        public int? AutoTransactionTypeID { get; set; }

        [ForeignKey("EmployeeID")]
        [InverseProperty("EmployeeTransactions")]
        public virtual Employee Employee { get; set; }
        [ForeignKey("ProjectID")]
        [InverseProperty("EmployeeTransactions")]
        public virtual Project Project { get; set; }
    }
}