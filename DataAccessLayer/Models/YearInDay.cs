﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Models
{
    [Keyless]
    public partial class YearInDay
    {
        [StringLength(50)]
        [Unicode(false)]
        public string DayID { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string DayInMonth { get; set; }
    }
}