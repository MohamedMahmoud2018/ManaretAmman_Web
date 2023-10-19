using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.DTO
{
    public class EmployeeLeavesInput
    {
        public int ID { get; set; }
        public int EmployeeID { get; set; }
        public int? LeaveTypeID { get; set; }
        [Required]
        public DateTime? LeaveDate { get; set; }
        [Required]
        public string FromTime { get; set; }
        [Required]
        public string ToTime { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModificationDate { get; set; }
        public int ProjectID { get; set; }
    }
}
