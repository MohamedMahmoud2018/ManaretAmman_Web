using DataAccessLayer.Auth;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using System;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLogicLayer.UnitOfWork;
using DataAccessLayer.Identity;
using BusinessLogicLayer.Services.ProjectProvider;

namespace BusinessLogicLayer.Services.Auth
{
    public class AuthService: IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;
        private IProjectProvider _projectProvider;
        static int _projectId;

        public AuthService(IConfiguration configuration, IUnitOfWork unit, IMapper mapper, IProjectProvider projectProvider) {
            _configuration = configuration;
            _unit = unit;
            _mapper = mapper;
            _projectProvider = projectProvider;
            _projectId=_projectProvider.GetProjectId();
        }
        
        public bool CheckIfValidUser(int userId)
        {
            bool isValid=_unit.UserRepository.GetFirstOrDefault(user=>user.UserID == userId&&user.ProjectID== _projectId) !=null;
            return isValid;
        }
        public int? IsHr(int userId)
        {
           var employee = _unit.EmployeeRepository.GetFirstOrDefault(emp=>emp.UserID==userId&&emp.ProjectID == _projectId);
            return employee is null ? null : employee.EmployeeID;
        }
        public string Login(LoginModel model)
        {
            if (IsValidUser(model.Username, model.Password, _projectId))
            {
                var token = GenerateJwtToken(model.Username);
              return   token ;
            }
            else
            {
                return null;
            }
            
        }

        private bool IsValidUser(string username, string password,int projectId)
        {
            var user = _unit.UserRepository.GetFirstOrDefault(user => user.UserName == username && user.ProjectID == projectId);
         return user is null?false:
              string.Compare(password, user.UserPassword)==0;
           // return true;
        }
            private string GenerateJwtToken(string username)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim("userName",username),
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
