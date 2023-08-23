using BusinessLogicLayer.Services.Auth;
using DataAccessLayer.Auth;
using DataAccessLayer.DTO;
using ManaretAmman.Models;
using Microsoft.AspNetCore.Mvc;

namespace ManaretAmman.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            this._authService = authService;
        }

        [HttpPost("Login")]
        public IApiResponse Login([FromBody]LoginModel model)
        {
            var result = _authService.Login(model);

            if (result == null)
            {
                return ApiResponse<AuthResponse>.Failure(result, new[] { "This User is not found" });
            }

            return ApiResponse<AuthResponse>.Success("data has been retrieved succussfully", result);
        }
    }
}
