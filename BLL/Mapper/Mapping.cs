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
            #region Employee

            CreateMap<Employee, EmployeeLookup>();

            #endregion


            #region Lookups

            CreateMap<LookupTable, LookupDto>();

            #endregion


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

            CreateMap<EmployeeVacation, EmployeeVacationInput>().ForMember(destination => destination.ID,
                options => options.MapFrom(source => source.EmployeeVacationID));

            CreateMap<EmployeeVacationInput, EmployeeVacation>().ForMember(destination => destination.EmployeeVacationID,
                    options => options.MapFrom(source => source.ID));

            CreateMap<EmployeeVacation, EmployeeVacationOutput>().ForMember(destination => destination.EmployeeName,
                options => options.MapFrom(source => source.Employee.EmployeeName)).ForMember(destination => destination.ID,
                options => options.MapFrom(source => source.EmployeeVacationID));

            #endregion


        }
    }
}
