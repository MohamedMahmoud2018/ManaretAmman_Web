using BusinessLogicLayer.Services.Auth;
using BusinessLogicLayer.Services.Balance;
using DataAccessLayer.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ManaretAmman.Controllers.Auth
{
    [Route("api/auth/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            this._authService = authService;
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody]LoginModel model)
        {
            var result = _authService.Login(model);
            if(result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}
