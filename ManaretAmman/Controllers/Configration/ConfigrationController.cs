using BusinessLogicLayer.Services.Configration;
using DataAccessLayer.DTO;
using ManaretAmman.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ManaretAmman.Controllers.Configration
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigrationController : ControllerBase
    {
        private readonly IConfigrationService _configrationService;
        public ConfigrationController(IConfigrationService configrationService)
        {
                _configrationService = configrationService;
        }
        [HttpGet("GetProjectUrl")]
        public async Task< IApiResponse<ConfigrationOutput>> GetProjectUrl(string projectCode)
        {
            var result =await _configrationService.GetProjectUrl(projectCode);
            return ApiResponse<ConfigrationOutput>.Success(result);
        }
    }
}
