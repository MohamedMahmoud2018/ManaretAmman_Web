﻿using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.DTO;

public class EmployeeVacationInput
{

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

}
