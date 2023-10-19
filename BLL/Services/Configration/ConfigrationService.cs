using BusinessLogicLayer.Exceptions;
using BusinessLogicLayer.UnitOfWork;
using DataAccessLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services.Configration
{
    public class ConfigrationService : IConfigrationService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ConfigrationService(IUnitOfWork unitOfWork)
        {
             _unitOfWork=unitOfWork;
        }


       public async Task<ConfigrationOutput> GetProjectUrl(string projectCode)
        {
            var project = _unitOfWork.ProjectRepository.Get(proj => proj.projectcode == projectCode).FirstOrDefault();
            if (project == null)
                throw new NotFoundException("project Code not found");

            return new ConfigrationOutput { ProjectId=project.ProjectID,ProjectUrl=project.apiurl.ToString()};
        }
    }
}
