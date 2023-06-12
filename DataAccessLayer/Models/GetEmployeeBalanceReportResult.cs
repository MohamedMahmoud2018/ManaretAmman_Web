﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Models
{
    public partial class GetEmployeeBalanceReportResult
    {
        public string PrintDate { get; set; }
        public string PrintTime { get; set; }
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public int? EmployeeNumber { get; set; }
        public int? CurrentBalance { get; set; }
        public int PreviousBalance { get; set; }
        public int EnableDelete { get; set; }
        public int EnableUpdate { get; set; }
        public int? ActualBalance { get; set; }
        public int? StartDate { get; set; }
        public int? EndDate { get; set; }
        public int YearID { get; set; }
        public int? Balance { get; set; }
        public int? SettingBalance { get; set; }
        public string AttendanceDate { get; set; }
        public string Amount { get; set; }
        public int? Amountasint { get; set; }
        public string CurrentBalanceTemp { get; set; }
        public string PreviousBalanceTemp { get; set; }
        public string CurrentBalanceMinutes { get; set; }
        public string BalanceTemp { get; set; }
        public string VacationCount { get; set; }
        public string TOtalLeave { get; set; }
        public string MissingCheckIn { get; set; }
        public string MissingCheckoutValue { get; set; }
        public string PlusHRTransaction { get; set; }
        public string MinusHRTransaction { get; set; }
        public int? Deducted { get; set; }
        public string companyname { get; set; }
        public string footertitle1 { get; set; }
        public string footertitle2 { get; set; }
        public string imagepath { get; set; }
        public string FirstBalance { get; set; }
        public string SecondBalance { get; set; }
        public string ThirdBalance { get; set; }
        public string FourthBalance { get; set; }
        public string FifthBalance { get; set; }
        public string SixthBalance { get; set; }
        public string SeventhBalance { get; set; }
        public string EighthBalance { get; set; }
        public string NinthBalance { get; set; }
        public string EarlyandMorningLeaves { get; set; }
        public string BalanceTemp2 { get; set; }
        public int? CuurentBalanceUpToDate { get; set; }
        public int? TypeID { get; set; }
    }
}
