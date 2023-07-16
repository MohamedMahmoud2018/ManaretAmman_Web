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
            #region EmployeeLeaves

                CreateMap<EmployeeLeaf, EmployeeLeavesInput>().ForMember(destination => destination.ID,
                    options => options.MapFrom(source => source.EmployeeLeaveID));

                CreateMap<EmployeeLeavesInput, EmployeeLeaf>().ForMember(destination => destination.EmployeeLeaveID,
                    options => options.MapFrom(source => source.ID));

                CreateMap<EmployeeLeaf, EmployeeLeavesOutput>().ForMember(destination => destination.EmployeeName,
                    options => options.MapFrom(source => source.Employee.EmployeeName))
                    .ForMember(destination => destination.ID,
                    options => options.MapFrom(source => source.EmployeeLeaveID));

            #endregion


            #region EmployeeVacations

                CreateMap<EmployeeVacation, EmployeeVacationInput>().ReverseMap();
                CreateMap<EmployeeVacation, EmployeeVacationOutput>().ForMember(destination => destination.EmployeeName,
                    options => options.MapFrom(source => source.Employee.EmployeeName));

            #endregion


            #region Employee

                CreateMap<Employee, EmployeeLookup>();

            #endregion


            #region Lookups

                CreateMap<LookupTable, LookupDto>();

            #endregion



        }
    }
}
