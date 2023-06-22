using AutoMapper;
using DataAccessLayer.DTO;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Mapper
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<EmployeeLeaf, EmployeeLeavesInput>().ReverseMap();
            CreateMap<EmployeeLeaf, EmployeeLeavesOutput>();
        }
    }
}
