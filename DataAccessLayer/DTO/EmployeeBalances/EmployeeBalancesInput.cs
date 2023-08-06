using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.DTO
{
    public class EmployeeBalancesInput
    {
        public int EmployeeID { get; set; }
        public int ProjectID { get; set; }
        public int YearID { get; set; }
        public int? Flag { get; set; } = 1;
        public int? Languageid { get; set; }
    }
}
