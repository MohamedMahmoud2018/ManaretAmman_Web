using AutoMapper;
using BusinessLogicLayer.Common;
using BusinessLogicLayer.Exceptions;
using BusinessLogicLayer.Extensions;
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
                       .PQuery(e => e.EmployeeLeaveID == id, include: e => e.Employee)
                       .FirstOrDefault();

            if (leave is null)
                throw new NotFoundException("data not found");

            var lookups = await _lookupsService.GetLookups(Constants.EmployeeLeaves, Constants.LeaveTypeID);

            var result = new EmployeeLeavesOutput
            {
                EmployeeLeaveID = leave.EmployeeLeaveID,
                EmployeeID      = leave.EmployeeID,
                EmployeeName    = leave.Employee.EmployeeName,
                LeaveTypeID     = leave.LeaveTypeID,
                LeaveType       = lookups.FirstOrDefault(e => leave.LeaveTypeID is not null
                                 && e.ColumnValue == leave.LeaveTypeID.ToString())?.ColumnDescription,
                LeaveDate       = leave.LeaveDate.ConvertFromUnixTimestampToDateTime(),
                FromTime        = leave.FromTime.ConvertFromMinutesToTimeString(),
                ToTime          = leave.ToTime.ConvertFromMinutesToTimeString()
            };

            return result;
        }

        public async Task<List<EmployeeLeavesOutput>> GetAll()
        {
            var leaves = _unitOfWork.EmployeeLeaveRepository.PQuery(include: e => e.Employee).ToList();

            var lookups = await _lookupsService.GetLookups(Constants.EmployeeLeaves, Constants.LeaveTypeID);

            var result = leaves.Select(item => new EmployeeLeavesOutput 
            {
                EmployeeLeaveID = item.EmployeeLeaveID,
                EmployeeID      = item.EmployeeID,
                EmployeeName    = item.Employee.EmployeeName,
                LeaveTypeID     = item.LeaveTypeID,
                LeaveType       = lookups.FirstOrDefault(e => item.LeaveTypeID is not null
                                 && e.ColumnValue == item.LeaveTypeID.ToString())?.ColumnDescription,
                LeaveDate       = item.LeaveDate.ConvertFromUnixTimestampToDateTime() ,
                FromTime        = item.FromTime.ConvertFromMinutesToTimeString(),
                ToTime          = item.ToTime.ConvertFromMinutesToTimeString()
            });

            return result.ToList();
        }

        public async Task Create(EmployeeLeavesInput model)
        {
            if (model == null)
                throw new NotFoundException("recieved data is missed");

            var timing = GetLeaveTimingInputs(model);

            model.LeaveDate = null;
            model.FromTime  = null;
            model.ToTime    = null;

            var employeeLeave = _mapper.Map<EmployeeLeaf>(model);

            employeeLeave.LeaveDate = timing.LeaveDate;
            employeeLeave.FromTime  = timing.FromTime;
            employeeLeave.ToTime    = timing.ToTime;

            employeeLeave.LeaveTypeID = null;

            await _unitOfWork.EmployeeLeaveRepository.PInsertAsync(employeeLeave);

             await _unitOfWork.SaveAsync();
        }

        public async Task Update(EmployeeLeavesInput employeeLeave)
        {
            var leave = _unitOfWork.EmployeeLeaveRepository.Get(emp => emp.EmployeeLeaveID == employeeLeave.EmployeeLeaveID)
                .FirstOrDefault();

            if (leave is null)
                throw new NotFoundException("Data Not Found");

            var timing = GetLeaveTimingInputs(employeeLeave);

            employeeLeave.LeaveDate = null;
            employeeLeave.FromTime  = null;
            employeeLeave.ToTime    = null;

            var updatedLeave = _mapper.Map<EmployeeLeavesInput, EmployeeLeaf>(employeeLeave);

            updatedLeave.LeaveDate = timing.LeaveDate;
            updatedLeave.FromTime  = timing.FromTime;
            updatedLeave.ToTime    = timing.ToTime;

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

        private (int? FromTime, int? ToTime, int? LeaveDate) GetLeaveTimingInputs(EmployeeLeavesInput model)
        {
            return (
                   FromTime: model.FromTime.ConvertFromTimeStringToMinutes() ,
                   ToTime: model.ToTime.ConvertFromTimeStringToMinutes(),
                   LeaveDate: model.LeaveDate.ConvertFromDateTimeToUnixTimestamp()
                ) ;
        }

    }
}
