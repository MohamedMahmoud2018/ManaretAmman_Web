using DataAccessLayer.Contracts;

namespace DataAccessLayer.DTO.Notification
{
    public class AcceptOrRejectNotifcationInput:IMustHaveProject
    {
        public int EmployeeId { get; set; }
        public int ApprovalStatusId { get; set; }
        public int CreatedBy { get; set; }
        public int ApprovalPageID { get; set; }
        public int ProjectID { get; set; }
        public int Id { get; set; }
        public int PrevilageType { get; set; }
        public int SendToLog { get; set; } = 0;
    }
}
