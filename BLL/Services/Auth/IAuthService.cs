using DataAccessLayer.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services.Auth
{
    public interface IAuthService
    {
        public string Login(LoginModel model);
    }
}
