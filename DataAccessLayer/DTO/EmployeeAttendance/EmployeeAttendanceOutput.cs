﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO.EmployeeAttendance
{
    public class EmployeeAttendanceOutput
    {
        public int? EmployeeNumber { get; set; }
        public string EmployeeName { get; set; }

        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Notes { get; set; }

        public string DayDesc { get; set; }
        public DateTime? AttendanceDate { get; set; }
        public string ShiftName { get; set; }


    }
}
