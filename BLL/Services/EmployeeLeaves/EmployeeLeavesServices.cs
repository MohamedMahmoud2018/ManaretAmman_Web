using AutoMapper;
using BusinessLogicLayer.Common;
using BusinessLogicLayer.Exceptions;
using BusinessLogicLayer.Extensions;
using BusinessLogicLayer.Services.Auth;
using BusinessLogicLayer.Services.Lookups;
using BusinessLogicLayer.Services.ProjectProvider;
using BusinessLogicLayer.UnitOfWork;
using DataAccessLayer.DTO;
using DataAccessLayer.DTO.EmployeeLeaves;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using UnauthorizedAccessException = BusinessLogicLayer.Exceptions.UnauthorizedAccessException;


namespace BusinessLogicLayer.Services.EmployeeLeaves;

internal class EmployeeLeavesService : IEmployeeLeavesService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILookupsService _lookupsService;
    private readonly IMapper _mapper;
    IProjectProvider _projectProvider;
    IAuthService _authService;
    readonly int userId;
    readonly int projecId;
    public EmployeeLeavesService(IUnitOfWork unityOfWork, ILookupsService lookupsService, IMapper mapper, IProjectProvider projectProvider, IAuthService authService)
    {
        _unitOfWork     = unityOfWork;
        _lookupsService = lookupsService;
        _mapper         = mapper;
        _projectProvider = projectProvider;
        _authService = authService;
        userId= _projectProvider.UserId();
        projecId = _projectProvider.GetProjectId();
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
                ID              = leave.EmployeeLeaveID,
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

    private static IQueryable<EmployeeLeaf> ApplyFilters(IQueryable<EmployeeLeaf> query, EmployeeLeaveFilter criteria)
    {
        if (criteria.EmployeeID != null)
            query = query.Where(e => e.EmployeeID == criteria.EmployeeID);

        if (criteria.LeaveTypeID != null)
            query = query.Where(e => e.LeaveTypeID == criteria.LeaveTypeID);

        if (criteria.LeaveDate != null)
            query = query.Where(e => e.LeaveDate == criteria.LeaveDate.DateToIntValue());

        if (criteria.FromTime != null)
            query = query.Where(e => e.FromTime == criteria.FromTime.ConvertFromTimeStringToMinutes());

        if (criteria.ToTime != null)
            query = query.Where(e => e.ToTime == criteria.ToTime.ConvertFromTimeStringToMinutes());
        if(criteria.LeaveDateTo != null && criteria.LeaveDateFrom!=null)
            query=query.Where(e=>e.LeaveDate>=criteria.LeaveDateFrom.DateToIntValue()&&e.LeaveDate<=criteria.LeaveDateTo.DateToIntValue());

        return query;

    }

    public async Task<PagedResponse<EmployeeLeavesOutput>> GetPage(PaginationFilter<EmployeeLeaveFilter> filter)
    {

        if (userId == -1) throw new UnauthorizedAccessException("Incorrect userId");
        if (!_authService.CheckIfValidUser(userId)) throw new UnauthorizedAccessException("Incorrect userId");
        int? employeeId = _authService.IsHr(userId);

        var query = from e in _unitOfWork.EmployeeRepository.PQuery()
                    join lt in _unitOfWork.LookupsRepository.PQuery() on e.DepartmentID equals lt.ID into ltGroup
                    from lt in ltGroup.DefaultIfEmpty()
                    join el in _unitOfWork.EmployeeLeaveRepository.PQuery() on e.EmployeeID equals el.EmployeeID
                    where e.ProjectID == 97 && (e.EmployeeID == employeeId || lt.EmployeeID == employeeId || employeeId==null)
                    select new EmployeeLeaf
                    {
                        Employee=e,
                        EmployeeID=e.EmployeeID,
                        approvalstatusid=el.approvalstatusid,
                        EmployeeLeaveID = el.EmployeeLeaveID,
                        LeaveTypeID = el.LeaveTypeID,
                        ProjectID = el.ProjectID,
                        LeaveDate = el.LeaveDate,
                        FromTime = el.FromTime,
                        ToTime = el.ToTime
                    };

        var xres=query.Where(e=>e.approvalstatusid !=null).ToList();

        //return new PagedResponse<EmployeeLeavesOutput>(null, 0, 0);

        if (filter.FilterCriteria != null)
            ApplyFilters(query, filter.FilterCriteria);

        var totalRecords = await query.CountAsync();

        var leaves = await query.Skip((filter.PageIndex - 1) * filter.Offset)
                    .Take(filter.Offset).ToListAsync();

        var lookups = await _lookupsService.GetLookups(Constants.EmployeeLeaves, Constants.LeaveTypeID);

        var approvals = await _lookupsService.GetLookups(Constants.Approvals, string.Empty);

        var result = leaves.Select(item => new EmployeeLeavesOutput
        {
            ID = item.EmployeeLeaveID,
            EmployeeID = item.EmployeeID,
            EmployeeName = item.Employee.EmployeeName,
            LeaveTypeID = item.LeaveTypeID,
            ProjectID = item.ProjectID,
            LeaveType = lookups.FirstOrDefault(e => item.LeaveTypeID is not null
                             && e.ID == item.LeaveTypeID)?.ColumnDescription,
            LeaveDate = item.LeaveDate.IntToDateValue(),
            FromTime = item.FromTime.ConvertFromMinutesToTimeString(),
            ToTime = item.ToTime.ConvertFromMinutesToTimeString(),
            ApprovalStatus = approvals.FirstOrDefault(e => e.ColumnValue == item.approvalstatusid.ToString())?.ColumnDescriptionAr
        }).ToList();

        return result.CreatePagedReponse(filter.PageIndex, filter.Offset, totalRecords);
    }

    public async Task Create(EmployeeLeavesInput model)
    {
        if (userId == -1) throw new UnauthorizedAccessException("Incorrect userId");
        if (!_authService.CheckIfValidUser(userId)) throw new UnauthorizedAccessException("Incorrect userId");
        if (model == null)
            throw new NotFoundException("recieved data is missed");

        var timing = GetLeaveTimingInputs(model);
        var LeaveDate = model.LeaveDate;
        model.LeaveDate = null;
        model.FromTime = null;
        model.ToTime = null;

        var employeeLeave = _mapper.Map<EmployeeLeaf>(model);

        employeeLeave.LeaveDate = LeaveDate.DateToIntValue();
        employeeLeave.FromTime = timing.FromTime;
        employeeLeave.ToTime = timing.ToTime;

        await _unitOfWork.EmployeeLeaveRepository.PInsertAsync(employeeLeave);

        await _unitOfWork.SaveAsync();
    }

    public async Task Update(EmployeeLeavesUpdate employeeLeave)
    {
        if (userId == -1) throw new UnauthorizedAccessException("Incorrect userId");
        if (!_authService.CheckIfValidUser(userId)) throw new UnauthorizedAccessException("Incorrect userId");

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

        leave.LeaveDate = employeeLeave.LeaveDate.DateToIntValue();// timing.LeaveDate;//
        leave.FromTime = timing.FromTime;
        leave.ToTime = timing.ToTime;
        leave.LeaveDate = employeeLeave.LeaveDate.DateToIntValue();
        leave.LeaveTypeID = employeeLeave.LeaveTypeID;



        await _unitOfWork.EmployeeLeaveRepository.PUpdateAsync(leave);

        await _unitOfWork.SaveAsync();

    }

    public async Task Delete(int employeeLeaveId)
    {
        if (userId == -1) throw new UnauthorizedAccessException("Incorrect userId");
        if (!_authService.CheckIfValidUser(userId)) throw new UnauthorizedAccessException("Incorrect userId");

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
               FromTime: model.FromTime.ConvertFromTimeStringToMinutes(),
               ToTime: model.ToTime.ConvertFromTimeStringToMinutes(),
               LeaveDate: model.LeaveDate.ConvertFromDateTimeToUnixTimestamp()
            );
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
