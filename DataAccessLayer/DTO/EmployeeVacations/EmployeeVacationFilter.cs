namespace DataAccessLayer.DTO.EmployeeVacations;

public class EmployeeVacationFilter
{
    public int? EmployeeID { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public int? VacationTypeId { get; set; }
}
