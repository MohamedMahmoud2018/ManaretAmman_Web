using AutoMapper;
using BusinessLogicLayer.Mapper;
using BusinessLogicLayer.UnitOfWork;
using DataAccessLayer.DTO;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services.Implementation
{
    internal class EmployeeLeafService : IEmployeeLeafService
    {
        private IUnitOfWork _unityOfWork;
        private IMapper _mapper;
        public EmployeeLeafService(IUnitOfWork unityOfWork, IMapper mapper) { 
        _unityOfWork = unityOfWork;
            _mapper = mapper;
        }
        public HttpStatusCode Create(EmployeeLeavesInput employee)
        {
            EmployeeLeaf employeeLeave =_mapper.Map<EmployeeLeaf>(employee);
            _unityOfWork.EmployeeLeafRepo.Insert(employeeLeave);
            _unityOfWork.Save();
            return HttpStatusCode.OK;
        }

        
        public HttpStatusCode delete(EmployeeLeafDelete employee)
        {
           EmployeeLeaf employeeToDelete= _unityOfWork.EmployeeLeafRepo.Get().Where(emp=>emp.EmployeeID==employee.EmployeeID&&emp.ProjectID==employee.ProjectID).FirstOrDefault();
            if (employeeToDelete != null)
            {
                _unityOfWork.EmployeeLeafRepo.Delete(employeeToDelete);
                return HttpStatusCode.OK;
            }
            else
                return HttpStatusCode.NoContent;
        }

        public EmployeeLeavesOutput Get(int id, int projectId)
        {
           var employee= _unityOfWork.EmployeeLeafRepo.Get(emp=>emp.EmployeeID==id && emp.ProjectID==projectId).FirstOrDefault();
            if (employee != null)
            {
                var selectedEmployeeLeave= _mapper.Map<EmployeeLeavesOutput>(employee);

                selectedEmployeeLeave.EmployeeLeaveName = _unityOfWork.LookupTableRepo
                    .Get(look=>look.ProjectID==projectId
                    &&Int32.Parse( look.ColumnValue)==selectedEmployeeLeave.LeaveTypeID
                    &&look.TableName== "EmployeeLeaves" 
                    && look.ColumnName== "LeaveTypeID").FirstOrDefault().ColumnDescription.ToString();

                selectedEmployeeLeave.EmployeeLeaveNameAr = _unityOfWork.LookupTableRepo
                    .Get(look => look.ProjectID == projectId
                    && Int32.Parse(look.ColumnValue) == selectedEmployeeLeave.LeaveTypeID
                    && look.TableName == "EmployeeLeaves"
                    && look.ColumnName == "LeaveTypeID").FirstOrDefault().ColumnDescriptionAr.ToString();
                return selectedEmployeeLeave;

            }
            return new EmployeeLeavesOutput();
        }

        public ICollection<EmployeeLeavesOutput> GetAll(int projectId)
        {
            var employees = _unityOfWork.EmployeeLeafRepo.Get(emp=>emp.ProjectID == projectId);
            return _mapper.Map<ICollection<EmployeeLeavesOutput>>(employees);
        }

        public HttpStatusCode Update(EmployeeLeavesInput employee)
        {
            EmployeeLeaf employeeToUpdate = _unityOfWork.EmployeeLeafRepo.Get().Where(emp => emp.EmployeeID == employee.EmployeeID && emp.ProjectID == employee.ProjectID).FirstOrDefault();
            if (employeeToUpdate != null)
            {
                _unityOfWork.EmployeeLeafRepo.Update(employeeToUpdate);
                return HttpStatusCode.OK;
            }
            else
                return HttpStatusCode.NoContent;
        }
    }
}
