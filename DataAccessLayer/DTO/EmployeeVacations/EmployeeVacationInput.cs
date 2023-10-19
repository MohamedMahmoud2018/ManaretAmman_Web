using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.DTO;

public class EmployeeVacationInput
{

    public int ID { get; set; }
    public int EmployeeID { get; set; }
    public int? VacationTypeID { get; set; }
    [Required]
    public DateTime? FromDate { get; set; }
    [Required]
    public DateTime? ToDate { get; set; }
    public string? Notes { get; set; }
    public int? DayCount { get; set; }
    public int? CreatedBy { get; set; }

    public DateTime? CreationDate { get; set; }
    public int? ModifiedBy { get; set; }

    public DateTime? ModificationDate { get; set; }
    public int ProjectID { get; set; }

}
