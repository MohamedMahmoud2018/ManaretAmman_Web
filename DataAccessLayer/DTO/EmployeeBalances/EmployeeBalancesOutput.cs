using DataAccessLayer.Models;

namespace DataAccessLayer.DTO
{
    public class EmployeeBalancesOutput
    {
        public EmployeeBalancesOutput() { }

        public int ProjectID { get; set; }
        public int  EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public int  EmployeeNumber { get; set; }
        public float CurrentBalance { get; set; }
        public float PreviousBalance { get; set; }
        public int  EnableDelete { get; set; }
        public int  EnableUpdate { get; set; }
        public float ActualBalance { get; set; }
        public float StartDate { get; set; }
        public float EndDate { get; set; }
        public float YearID { get; set; }
        public float Balance { get; set; }
        public float SettingBalance { get; set; }
        public float CurrentBalanceTemp { get; set; }
        public float PreviousBalanceTemp { get; set; }
        public float CurrentBalanceMinutes { get; set; }
        public float BalanceTemp { get; set; }
        public float VacationCount { get; set; }
        public float TOtalLeave { get; set; }
        public float MissingCheckIn { get; set; }
        public float MissingCheckoutValue { get; set; }
        public float PlusHRTransaction { get; set; }
        public float MinusHRTransaction { get; set; }
        public float Deducted { get; set; }
        public float companyname { get; set; }
        public float footertitle1 { get; set; }
        public float footertitle2 { get; set; }
        public float imagepath { get; set; }
        public float FirstBalance { get; set; }
        public float SecondBalance { get; set; }
        public float ThirdBalance { get; set; }
        public float FourthBalance { get; set; }
        public float FifthBalance { get; set; }
        public float SixthBalance { get; set; }
        public float SeventhBalance { get; set; }
        public float EighthBalance { get; set; }
        public float NinthBalance { get; set; }
        public float EarlyandMorningLeaves { get; set; }
        public float BalanceTemp2 { get; set; }
        public float CuurentBalanceUpToDate { get; set; }
    }
}
