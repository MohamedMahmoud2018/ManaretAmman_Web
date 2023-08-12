namespace DataAccessLayer.DTO.EmployeeLeaves;

public class EmployeeLeaveFilter
{
    public int? EmployeeID { get; set; }

    public int? LeaveTypeID { get; set; }

    public DateTime? LeaveDate { get; set; }

    public string FromTime { get; set; }

    public string ToTime { get; set; }
}
