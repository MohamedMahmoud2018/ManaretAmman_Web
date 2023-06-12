﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Models
{
    [Table("EmployeeLanguage")]
    public partial class EmployeeLanguage
    {
        [Key]
        public int EmployeeID { get; set; }
        [Key]
        public int LanguageID { get; set; }
        public int? ReadStatus { get; set; }
        public int? WriteStatus { get; set; }
        public int? SpeakStatus { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreationDate { get; set; }
        public int? ModifiedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModificationDate { get; set; }
        [Key]
        public int ProjectID { get; set; }

        [ForeignKey("EmployeeID")]
        [InverseProperty("EmployeeLanguages")]
        public virtual Employee Employee { get; set; }
        [ForeignKey("ProjectID")]
        [InverseProperty("EmployeeLanguages")]
        public virtual Project Project { get; set; }
    }
}