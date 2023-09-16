using DataAccessLayer.Auth;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using System.Text;
using BusinessLogicLayer.UnitOfWork;
using BusinessLogicLayer.Services.ProjectProvider;
using DataAccessLayer.DTO;
using BusinessLogicLayer.Exceptions;

namespace BusinessLogicLayer.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unit;
        static int _projectId;

        public AuthService(IConfiguration configuration, IUnitOfWork unit, IProjectProvider projectProvider)
        {
            _unit = unit;
            _configuration = configuration;
            _projectId = projectProvider.GetProjectId();
        }

        
        public AuthResponse Login(LoginModel model)
        {
            int userId = _unit.UserRepository.GetFirstOrDefault(user => user.UserName == model.Username && user.ProjectID == _projectId).UserID;
            if (!IsValidUser(model.Username, model.Password, _projectId))
                return null;

            var token = GenerateJwtToken(model.Username,userId);

            return new AuthResponse { Token = token };

        }

        
        private bool IsValidUser(string username, string password,int projectId)
        {
            var user = _unit.UserRepository.GetFirstOrDefault(user => user.UserName == username && user.ProjectID == projectId);
         return user is null?false:
              string.Compare(password, user.UserPassword)==0;
           
        }
        public int GetUserType(int userId, int employeeId)
        {
            if (IsHr(userId) is null)
                return 2;
            else if(IsHr(userId)==employeeId) return 3;
            else return 1;


        }
        public bool CheckIfValidUser(int userId)
        {
            bool isValid = _unit.UserRepository.GetFirstOrDefault(user => user.UserID == userId && user.ProjectID == _projectId) != null;
            return isValid;
        }
        public int? IsHr(int userId)
        {
            var employee = _unit.EmployeeRepository.GetFirstOrDefault(emp => emp.UserID == userId && emp.ProjectID == _projectId);

            return employee is not null ? employee.EmployeeID : null;
        }
        private string GenerateJwtToken(string username,int userId)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("userName",username),
                new Claim("userId",userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                _configuration["Jwt:ValidIssuer"],
                _configuration["Jwt:ValidAudience"],
                claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpireMinutes"])),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
