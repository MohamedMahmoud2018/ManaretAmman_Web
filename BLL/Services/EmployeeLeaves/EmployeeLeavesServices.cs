using AutoMapper;
using BusinessLogicLayer.Common;
using BusinessLogicLayer.Exceptions;
using BusinessLogicLayer.Extensions;
using BusinessLogicLayer.Services.Lookups;
using BusinessLogicLayer.UnitOfWork;
using DataAccessLayer.DTO;
using DataAccessLayer.DTO.EmployeeLeaves;
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
                ID = leave.EmployeeLeaveID,
                EmployeeID      = leave.EmployeeID,
                EmployeeName    = leave.Employee.EmployeeName,
                LeaveTypeID     = leave.LeaveTypeID,
                LeaveType       = lookups.FirstOrDefault(e => leave.LeaveTypeID is not null
                                 && e.ID == leave.LeaveTypeID)?.ColumnDescription,
                LeaveDate       = leave.LeaveDate.IntToDateValue(),
                FromTime        = leave.FromTime.ConvertFromMinutesToTimeString(),
                ToTime          = leave.ToTime.ConvertFromMinutesToTimeString()
                
            };

            return result;
        }

        public async Task<List<EmployeeLeavesOutput>> GetAll()
        {
            var leaves = _unitOfWork.EmployeeLeaveRepository.PQuery(include: e => e.Employee).ToList();

            var lookups = await _lookupsService.GetLookups(Constants.EmployeeLeaves, Constants.LeaveTypeID);

            var approvals = await _lookupsService.GetLookups(Constants.Approvals, string.Empty);

            var result = leaves.Select(item => new EmployeeLeavesOutput 
            {
                ID              = item.EmployeeLeaveID,
                ProjectID       = item.ProjectID,
                EmployeeID      = item.EmployeeID,
                EmployeeName    = item.Employee.EmployeeName,
                LeaveTypeID     = item.LeaveTypeID,
                LeaveType       = lookups.FirstOrDefault(e => item.LeaveTypeID is not null
                                 && e.ID == item.LeaveTypeID)?.ColumnDescription,
                LeaveDate       = item.LeaveDate.IntToDateValue() ,
                FromTime        = item.FromTime.ConvertFromMinutesToTimeString(),
                ToTime          = item.ToTime.ConvertFromMinutesToTimeString()   ,
                ApprovalStatus  = approvals.FirstOrDefault(e => e.ColumnValue == item.approvalstatusid.ToString())?.ColumnDescription
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

            employeeLeave.LeaveDate    = model.LeaveDate.DateToIntValue();
            employeeLeave.FromTime     = timing.FromTime;
            employeeLeave.ToTime       = timing.ToTime;

            await _unitOfWork.EmployeeLeaveRepository.PInsertAsync(employeeLeave);

             await _unitOfWork.SaveAsync();
        }

        public async Task Update(EmployeeLeavesUpdate employeeLeave)
        {
            var leave = _unitOfWork.EmployeeLeaveRepository.Get(emp => emp.EmployeeLeaveID == employeeLeave.ID)
                .FirstOrDefault();

            if (leave is null)
                throw new NotFoundException("Data Not Found");

            var timing = GetLeaveTimingInputs(employeeLeave);

            leave.LeaveDate =employeeLeave.LeaveDate.DateToIntValue();// timing.LeaveDate;//
            leave.FromTime = timing.FromTime;
            leave.ToTime = timing.ToTime;
            leave.ModificationDate = DateTime.Now;
            leave.LeaveTypeID= employeeLeave.LeaveTypeID;

            await _unitOfWork.EmployeeLeaveRepository.UpdateAsync(leave);

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

        private (int? FromTime, int? ToTime, int? LeaveDate) GetLeaveTimingInputs(EmployeeLeavesUpdate model)
        {
            return (
                   FromTime: model.FromTime.ConvertFromTimeStringToMinutes(),
                   ToTime: model.ToTime.ConvertFromTimeStringToMinutes(),
                   LeaveDate: model.LeaveDate.ConvertFromDateTimeToUnixTimestamp()
                );
        }

    }
}
