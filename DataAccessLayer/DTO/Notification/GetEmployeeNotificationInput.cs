using DataAccessLayer.Contracts;

namespace DataAccessLayer.DTO.Notification
{
    public class GetEmployeeNotificationInput
    {
        public int UserId { get; set; }
        public int LanguageId { get; set; } = 1;
        public DateTime? Fromdate { get; set; }
        public DateTime? ToDate { get; set; }
        public int EmployeeID { get; set; }
    }
}
