using AutoMapper;
using BusinessLogicLayer.Exceptions;
using BusinessLogicLayer.UnitOfWork;
using DataAccessLayer.DTO.Employees;

namespace BusinessLogicLayer.Services.Employees;

internal class EmployeeService : IEmployeeService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public EmployeeService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper     = mapper;
    }

    public async Task<List<EmployeeLookup>> GetList()
    {
        //var query = from E in _unitOfWork.EmployeeRepository.Get()
        //            join LT in _unitOfWork.LookupsRepository.Get() on E.DepartmentID equals LT.ID into tempLT
        //            from LT in tempLT.DefaultIfEmpty()
        //            where E.ProjectID == 97 && (E.EmployeeID == 1815 || LT.EmployeeID == 1815)
        //            select new
        //            {
        //                E.EmployeeID,
        //                E.EmployeeName,
        //                E.EmployeeNumber,
        //                DepartmentID = E.DepartmentID
        //            };

        //var result1 = query.ToList();

        var employees = _unitOfWork.EmployeeRepository.PQuery().ToList();

        if (employees is null)
        {
            throw new NotFoundException("data is missing");
        }

        var result = _mapper.Map<List<EmployeeLookup>>(employees);

        return result;
    }
}
