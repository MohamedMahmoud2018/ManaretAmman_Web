namespace DataAccessLayer.DTO.EmployeeLeaves;

public class EmployeeLeaveFilter
{
    public int? EmployeeID { get; set; }

    public int? LeaveTypeID { get; set; }

    public DateTime? LeaveDate { get; set; }
    public DateTime? LeaveDateFrom { get; set; }
    public DateTime? LeaveDateTo { get; set; }

    public string FromTime { get; set; } = string.Empty;

    public string ToTime { get; set; } = string.Empty;
}
