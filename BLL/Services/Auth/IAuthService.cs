using DataAccessLayer.Auth;
using DataAccessLayer.DTO;

namespace BusinessLogicLayer.Services.Auth
{
    public interface IAuthService
    {
        public AuthResponse Login(LoginModel model);
        public bool ChangePassword( ChangePasswordModel model);
        public bool IsValidUser(int userId);
        public int? IsHr(int userId);
       /// <summary>
       /// get user privige type 1=>manager ,2 hr , 3 employee
       /// </summary>
       /// <param name="userId"></param>
       /// <param name="employeeId"></param>
       /// <returns></returns>
        public int GetUserType(int userId,int employeeId);
    }
}
