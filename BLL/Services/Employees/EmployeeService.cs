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
        var employees = _unitOfWork.EmployeeRepository.PQuery().ToList();

        if (employees is null)
        {
            throw new NotFoundException("data is missing");
        }

        var result = _mapper.Map<List<EmployeeLookup>>(employees);

        return result;
    }
}
