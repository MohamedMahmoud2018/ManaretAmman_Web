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
    }
}
