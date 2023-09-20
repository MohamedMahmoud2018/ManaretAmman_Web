namespace DataAccessLayer.DTO.EmployeeAttendance
{
    public class EmployeeAttendanceInput
    {
        public DateTime? ToDate { get; set; }
        public DateTime? FromDate { get; set; }
        public int? Flag { get; set; } = 7;
        public int? LanguageID { get; set; } = 1;
        public int? EmployeeID { get; set; }
        public int? UserId { get; set; }
        public int? YearId { get; set; }
    }
}
