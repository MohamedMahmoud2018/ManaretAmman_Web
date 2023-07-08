using AutoMapper;
using BusinessLogicLayer.Common;
using BusinessLogicLayer.Exceptions;
using BusinessLogicLayer.Services.Lookups;
using BusinessLogicLayer.UnitOfWork;
using DataAccessLayer.DTO;
using DataAccessLayer.Models;

namespace BusinessLogicLayer.Services.EmployeeLeaves
{
    internal class EmployeeLeavesService : IEmployeeLeavesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILookupsService _lookupsService;
        private readonly IMapper _mapper;
        public EmployeeLeavesService(IUnitOfWork unityOfWork, ILookupsService lookupsService, IMapper mapper)
        {
            _unitOfWork     = unityOfWork;
            _lookupsService = lookupsService;
            _mapper         = mapper;
        }
        public async Task<EmployeeLeavesOutput> Get(int id)
        {
            var leave = _unitOfWork.EmployeeLeaveRepository
                       .Get(e => e.EmployeeLeaveID == id)
                       .FirstOrDefault();

            if (leave is null)
                throw new NotFoundException("data not found");

            var lookups = await _lookupsService.GetLookups(Constants.EmployeeLeaves, Constants.LeaveTypeID);

            var result = _mapper.Map<EmployeeLeaf, EmployeeLeavesOutput>(leave);

            result.LeaveType = lookups.FirstOrDefault(e => e.ColumnValue == result.LeaveTypeID.ToString()).ColumnDescription;

            return result;
        }

        public async Task<List<EmployeeLeavesOutput>> GetAll()
        {
            var leaves = _unitOfWork.EmployeeLeaveRepository.Get(includes: e => e.Employee).ToList();

            var lookups = await _lookupsService.GetLookups(Constants.EmployeeLeaves, Constants.LeaveTypeID);

            var result = _mapper.Map<List<EmployeeLeaf>, List<EmployeeLeavesOutput>>(leaves);

            foreach (var item in result)
            {
                item.LeaveType = lookups.FirstOrDefault(e => e.ColumnValue == item.LeaveTypeID.ToString()).ColumnDescription;
            }

            return result;
        }

        public async Task Create(EmployeeLeavesInput model)
        {
            if (model == null)
                throw new NotFoundException("recieved data is missed");

            var employeeLeave = _mapper.Map<EmployeeLeaf>(model);

            await _unitOfWork.EmployeeLeaveRepository.InsertAsync(employeeLeave);

             await _unitOfWork.SaveAsync();
        }

        public async Task Update(EmployeeLeavesInput employeeLeave)
        {
            var leave = _unitOfWork.EmployeeLeaveRepository.Get(emp => emp.EmployeeLeaveID == employeeLeave.EmployeeLeaveID)
                .FirstOrDefault();

            if (leave is null)
                throw new NotFoundException("Data Not Found");

            var updatedLeave = _mapper.Map<EmployeeLeavesInput, EmployeeLeaf>(employeeLeave);

            await _unitOfWork.EmployeeLeaveRepository.UpdateAsync(updatedLeave);

            await _unitOfWork.SaveAsync();

        }

        public async Task Delete( int employeeLeaveId)
        {
            var leave = _unitOfWork.EmployeeLeaveRepository
                        .Get(e => e.EmployeeLeaveID == employeeLeaveId)
                        .FirstOrDefault();

            if (leave is null)
                throw new NotFoundException("Data Not Found");

            _unitOfWork.EmployeeLeaveRepository.Delete(leave);

            await _unitOfWork.SaveAsync();

        }
    }
}
