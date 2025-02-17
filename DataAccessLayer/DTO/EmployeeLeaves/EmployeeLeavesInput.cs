﻿namespace DataAccessLayer.DTO
{
    public class EmployeeLeavesInput
    {
        public int ID { get; set; }
        public int EmployeeID { get; set; }
        public int? LeaveTypeID { get; set; }
        public DateTime? LeaveDate { get; set; }
        public string FromTime { get; set; }
        public string ToTime { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModificationDate { get; set; }
        public int ProjectID { get; set; }
    }
}
