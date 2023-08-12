namespace DataAccessLayer.DTO.EmployeeLoans;

public class EmployeeLoanFilter
{
    public int? EmployeeID { get; set; }

    public DateTime? LoanDate { get; set; }

    public int? LoanTypeId { get; set; }
}
