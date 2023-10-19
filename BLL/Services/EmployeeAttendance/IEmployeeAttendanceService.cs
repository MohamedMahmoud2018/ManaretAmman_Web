using BusinessLogicLayer.Common;
using DataAccessLayer.DTO.EmployeeAttendance;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services.EmployeeAttendance
{
    public interface IEmployeeAttendanceService
    {
        public Task<PagedResponse<EmployeeAttendanceOutput>> GetEmployeeAttendance(PaginationFilter<EmployeeAttendanceInput> filter);
    }
}
