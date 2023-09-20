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
using DataAccessLayer.Models;

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
            
            if (!IsValidUser(model.Username, model.Password, _projectId))
                return null;
            var user = _unit.UserRepository.GetFirstOrDefault(user =>string.Equals( user.UserName , model.Username) && user.ProjectID == _projectId);
            if (user == null) return null;
            var employeeName = _unit.EmployeeRepository.GetFirstOrDefault(emp=>emp.UserID==user.UserID)!=null?
               _unit.EmployeeRepository.GetFirstOrDefault(emp => emp.UserID == user.UserID).EmployeeName:"HR" ;
            var token = GenerateJwtToken(model.Username,user.UserID, employeeName);

            return new AuthResponse { Token = token };

        }

        
        private bool IsValidUser(string username, string password,int projectId)
        {
            var user = _unit.UserRepository.GetFirstOrDefault(user => string.Equals(user.UserName, username) && user.ProjectID == projectId);
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
        public bool IsValidUser(int userId)
        {
            bool isValid = _unit.UserRepository.GetFirstOrDefault(user => user.UserID == userId && user.ProjectID == _projectId) != null;
            return isValid;
        }
        public int? IsHr(int userId)
        {
            var employee = _unit.EmployeeRepository.GetFirstOrDefault(emp => emp.UserID == userId && emp.ProjectID == _projectId);

            return employee is not null ? employee.EmployeeID : null;
        }
        public bool ChangePassword(ChangePasswordModel model)
        {
          bool isvalid=  this.IsValidUser(model.Username, model.Password, _projectId);
            if (isvalid) {
                var user = _unit.UserRepository.GetFirstOrDefault(user => user.UserName == model.Username && user.ProjectID == _projectId);
                user.UserPassword = model.NewPassword;
                _unit.UserRepository.Update(user);
                _unit.Save();
                return true;
            }
            return false;
        }
        private string GenerateJwtToken(string username,int userId,string employeeName)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("userName",username),
                new Claim("userId",userId.ToString()),
                new Claim("employeeName",employeeName),
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
