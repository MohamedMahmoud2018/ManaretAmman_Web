using DataAccessLayer.Auth;
using DataAccessLayer.DTO;

namespace BusinessLogicLayer.Services.Auth
{
    public interface IAuthService
    {
        public AuthResponse Login(LoginModel model);
        public bool CheckIfValidUser(int userId);
        public int? IsHr(int userId);
    }
}
