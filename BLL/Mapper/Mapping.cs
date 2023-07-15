using AutoMapper;
using DataAccessLayer.DTO;
using DataAccessLayer.DTO.Employees;
using DataAccessLayer.Models;

namespace BusinessLogicLayer.Mapper
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<EmployeeLeaf, EmployeeLeavesInput>().ReverseMap();
            CreateMap<EmployeeLeaf, EmployeeLeavesOutput>().ForMember(destination => destination.EmployeeName,
                options => options.MapFrom(source => source.Employee.EmployeeName));
            
            CreateMap<EmployeeVacation, EmployeeVacationInput>().ReverseMap();
            CreateMap<EmployeeVacation, EmployeeVacationOutput>().ForMember(destination => destination.EmployeeName,
                options => options.MapFrom(source => source.Employee.EmployeeName));

            CreateMap<LookupTable, LookupDto>();

            CreateMap<Employee, EmployeeLookup>();
        }
    }
}
