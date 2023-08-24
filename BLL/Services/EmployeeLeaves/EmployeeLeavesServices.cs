using AutoMapper;
using BusinessLogicLayer.Common;
using BusinessLogicLayer.Exceptions;
using BusinessLogicLayer.Extensions;
using BusinessLogicLayer.Services.Auth;
using BusinessLogicLayer.Services.Lookups;
using BusinessLogicLayer.Services.Notification;
using BusinessLogicLayer.Services.ProjectProvider;
using BusinessLogicLayer.UnitOfWork;
using DataAccessLayer.DTO;
using DataAccessLayer.DTO.EmployeeLeaves;
using DataAccessLayer.DTO.Notification;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using UnauthorizedAccessException = BusinessLogicLayer.Exceptions.UnauthorizedAccessException;


namespace BusinessLogicLayer.Services.EmployeeLeaves;

internal class EmployeeLeavesService : IEmployeeLeavesService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILookupsService _lookupsService;
    private readonly IMapper _mapper;
    readonly IProjectProvider _projectProvider;
    readonly IAuthService _authService;
    readonly INotificationsService _iNotificationsService;
    readonly int _userId;
    readonly int _projecId;
    public EmployeeLeavesService(IUnitOfWork unityOfWork, ILookupsService lookupsService, IMapper mapper, IProjectProvider projectProvider, IAuthService authService,INotificationsService iNotificationsService)
    {
        _unitOfWork     = unityOfWork;
        _lookupsService = lookupsService;
        _mapper         = mapper;
        _projectProvider = projectProvider;
        _authService = authService;
        _iNotificationsService = iNotificationsService;
        _userId = _projectProvider.UserId();
        _projecId = _projectProvider.GetProjectId();
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
        if (criteria.ToDate != null && criteria.FromDate != null)
        {
            
            query = query.Where(e => e.LeaveDate >= criteria.FromDate.DateToIntValue() && e.LeaveDate <= criteria.ToDate.DateToIntValue());

        }


        return query;

    }

    public async Task<PagedResponse<EmployeeLeavesOutput>> GetPage(PaginationFilter<EmployeeLeaveFilter> filter)
    {

        if (_userId == -1) throw new UnauthorizedAccessException("Incorrect userId");
        if (!_authService.CheckIfValidUser(_userId)) throw new UnauthorizedAccessException("Incorrect userId");
        int? employeeId = _authService.IsHr(_userId);

        var query = from e in _unitOfWork.EmployeeRepository.PQuery()
                    join lt in _unitOfWork.LookupsRepository.PQuery() on e.DepartmentID equals lt.ID into ltGroup
                    from lt in ltGroup.DefaultIfEmpty()
                    join el in _unitOfWork.EmployeeLeaveRepository.PQuery() on e.EmployeeID equals el.EmployeeID
                    where e.ProjectID == _projecId && lt.ProjectID==_projecId && el.ProjectID == _projecId && (e.EmployeeID == employeeId || lt.EmployeeID == employeeId || employeeId==null)
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

       
        if (filter.FilterCriteria != null)
            ApplyFilters(query, filter.FilterCriteria);

        var totalRecords = await query.CountAsync();
        var x = query.ToList();
        await Console.Out.WriteLineAsync(filter.FilterCriteria.ToDate.DateToIntValue().ToString());
        foreach (var item in query.ToList())
        {
            Console.WriteLine(item.LeaveDate);
            Console.WriteLine(item.LeaveDate >= filter.FilterCriteria.FromDate.DateToIntValue()&&item.LeaveDate <= filter.FilterCriteria.ToDate.DateToIntValue());
        }
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
        if (_userId == -1) throw new UnauthorizedAccessException("Incorrect userId");
        if (!_authService.CheckIfValidUser(_userId)) throw new UnauthorizedAccessException("Incorrect userId");
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
        var insertedPKValue = employeeLeave.EmployeeLeaveID;
       await sendToNotification(employeeLeave.EmployeeID, insertedPKValue);
    }
   async Task sendToNotification(int employeeId,int PKID)
    {
        int privigeType = _authService.GetUserType(_userId, employeeId);
        AcceptOrRejectNotifcationInput model = new AcceptOrRejectNotifcationInput() { 
        ProjectID=_projecId,
        CreatedBy=_userId,
        EmoloyeeId=employeeId,
        ApprovalStatusId=0,
        SendToLog=0,
        Id=PKID,
        ApprovalPageID=2,
        PrevilageType= privigeType
        };
       await _iNotificationsService.AcceptOrRejectNotificationsAsync(model);
    }
    public async Task Update(EmployeeLeavesUpdate employeeLeave)
    {
        if (_userId == -1) throw new UnauthorizedAccessException("Incorrect userId");
        if (!_authService.CheckIfValidUser(_userId)) throw new UnauthorizedAccessException("Incorrect userId");

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
        if (_userId == -1) throw new UnauthorizedAccessException("Incorrect userId");
        if (!_authService.CheckIfValidUser(_userId)) throw new UnauthorizedAccessException("Incorrect userId");

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
