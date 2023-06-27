using DataAccessLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public interface IEmployeeLeafService
    {
        HttpStatusCode Create(EmployeeLeavesInput employee);
        HttpStatusCode Update(EmployeeLeavesInput employee);
        HttpStatusCode delete(EmployeeLeafDelete employee);
        EmployeeLeavesOutput Get(int id,int projectId);
        ICollection<EmployeeLeavesOutput> GetAll(int projectId);
    }
}
