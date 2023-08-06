namespace DataAccessLayer.DTO
{
    public class EmployeeLeavesOutput
    {
        public EmployeeLeavesOutput() { }
        public int ID { get; set; }
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public int? LeaveTypeID { get; set; }
        public string LeaveType { get; set; }
        public string LeaveTypeAr { get; set; }
        public DateTime? LeaveDate { get; set; }
        public string FromTime { get; set; }
        public string ToTime { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? ModifiedBy { get; set; }
        public string ModifiedName { get; set; }
        public DateTime? ModificationDate { get; set; }
        public int ProjectID { get; set; }
        public string ApprovalStatus { get; set; }
        //public int? statusid { get; set; }
        //public int? approvalstatusid { get; set; }
    }
}
