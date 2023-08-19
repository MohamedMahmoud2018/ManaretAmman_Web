using DataAccessLayer.Contracts;

namespace DataAccessLayer.DTO.Notification
{
    public class GetEmployeeNotificationInput
    {
        public int UserId { get; set; }
        public DateTime? Fromdate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
