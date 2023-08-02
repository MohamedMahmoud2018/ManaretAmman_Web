using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.DTO;

public class EmployeeVacationOutput
{
    public EmployeeVacationOutput() { }

    public int ID { get; set; }
    public int EmployeeID { get; set; }
    public int? VacationTypeID { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    [StringLength(1000)]
    public string Notes { get; set; }
    public int? DayCount { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime? CreationDate { get; set; }
    public int? ModifiedBy { get; set; }
    public DateTime? ModificationDate { get; set; }
    public int ProjectID { get; set; }
    public int? StatusID { get; set; }
    public string EmployeeName { get; set; }
    public string VacationType { get; set; }
    public string VacationTypeAr { get; set; }
    public string VacationTypeEn { get; set; }
    public string ModifiedName { get; set; }
    public string CreatedName { get; set; }
    public string ApprovalStatus { get; set; }

    //public int? statusid { get; set; }
    //public int? approvalstatusid { get; set; }
}
