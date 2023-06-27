using BusinessLogicLayer.Services;
using DataAccessLayer.DTO;
using ManaretAmman.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ManaretAmman.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeLeavesController : ControllerBase
    {
        private IEmployeeLeafService _employeeLeafService;
        public EmployeeLeavesController(IEmployeeLeafService employeeLeafService) { 
        _employeeLeafService = employeeLeafService;
        }

        [Route("Create")]
        [HttpPost]
       public IApiResponse Create(EmployeeLeavesInput employee)
        {
            if (_employeeLeafService.Create(employee) == HttpStatusCode.OK)
                return ApiResponse.Success();
            else
                return ApiResponse.Failure("Error occured");
        }

        [Route("Update")]
        [HttpPost]
        public IApiResponse Update(EmployeeLeavesInput employee)
        {
            if (_employeeLeafService.Update(employee) == HttpStatusCode.OK)
                return ApiResponse.Success();
            else
                return ApiResponse.Failure("Error occured");
        }

        [Route("Delete")]
        [HttpPost]
        public IApiResponse Delete(EmployeeLeafDelete employee)
        {
            if (_employeeLeafService.delete(employee) == HttpStatusCode.OK)
                return ApiResponse.Success();
            else
                return ApiResponse.Failure("Error occured");
        }

        [Route("Get")]
        [HttpPost]
        public IApiResponse Get(int id, int projectId)
        {
            if (_employeeLeafService.Get(id,projectId) != null)
            {
               return ApiResponse<EmployeeLeavesOutput>.Success("", _employeeLeafService.Get(id, projectId));
            }
            else
                return ApiResponse.Failure("Error occured");
        }

        [Route("GetAll")]
        [HttpPost]
        public IApiResponse GetAll(int projectId)
        {
            if (_employeeLeafService.GetAll(projectId) != null)
            {
                return ApiResponse<ICollection<EmployeeLeavesOutput>>.Success("", _employeeLeafService.GetAll(projectId));
            }
            else
                return ApiResponse.Failure("Error occured");
        }
    }
}
