namespace DataAccessLayer.DTO
{
    public class EmployeeLeavesOutput
    {
        public EmployeeLeavesOutput() { }
        public int EmployeeLeaveID { get; set; }
        public string EmployeeLeaveName { get; set; } 
        public string EmployeeLeaveNameAr { get; set; }
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }  
        public int? LeaveTypeID { get; set; }
        public int? LeaveDate { get; set; }
        public int? FromTime { get; set; }
        public int? ToTime { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? ModifiedBy { get; set; }
        public string ModifiedName { get; set; }
        public DateTime? ModificationDate { get; set; }
        public int? BySystem { get; set; }
        public int ProjectID { get; set; }
        //public int? statusid { get; set; }
        //public int? approvalstatusid { get; set; }
    }
}
