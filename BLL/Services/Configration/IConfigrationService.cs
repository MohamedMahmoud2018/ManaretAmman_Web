using DataAccessLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services.Configration
{
    public interface IConfigrationService
    {
        public Task<ConfigrationOutput> GetProjectUrl(string projectCode);
    }
}
