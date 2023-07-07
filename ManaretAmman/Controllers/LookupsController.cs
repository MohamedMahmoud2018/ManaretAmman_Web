using BusinessLogicLayer.Services.Lookups;
using DataAccessLayer.DTO;
using ManaretAmman.Models;
using Microsoft.AspNetCore.Mvc;

namespace ManaretAmman.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LookupsController : ControllerBase
    {
        private readonly ILookupsService _lookupsService;
        public LookupsController(ILookupsService lookupsService)
        => _lookupsService = lookupsService;

        [HttpGet]
        public async Task<IApiResponse> Get(string tableName, string columnName)
        {
            var result = await _lookupsService.GetLookups(tableName, columnName);
            return ApiResponse<IList<LookupDto>>.Success("data has been retrieved succussfully",result);
        }
    }
}
