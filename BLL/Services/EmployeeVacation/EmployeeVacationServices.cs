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
using DataAccessLayer.DTO.EmployeeVacations;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using UnauthorizedAccessException = BusinessLogicLayer.Exceptions.UnauthorizedAccessException;

namespace BusinessLogicLayer.Services.EmployeeVacations
{
    internal class EmployeeVacationService : IEmployeeVacationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILookupsService _lookupsService;
        private readonly IMapper _mapper;
        readonly IProjectProvider _projectProvider;
        readonly IAuthService _authService;
        readonly INotificationsService _iNotificationsService;
        readonly int _userId;
        readonly int _projecId;
        public EmployeeVacationService(IUnitOfWork unityOfWork, ILookupsService lookupsService, IMapper mapper, IProjectProvider projectProvider, IAuthService authService, INotificationsService iNotificationsService)
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
        public async Task<EmployeeVacationOutput> Get(int id)
        {
            var Vacation = _unitOfWork.EmployeeVacationRepository
                       .PQuery(e => e.EmployeeVacationID == id, include: e => e.Employee)
                       .FirstOrDefault();

            if (Vacation is null)
                throw new NotFoundException("data not found");

            var lookups = await _lookupsService.GetLookups(Constants.VacationType, Constants.VacationTypeId);

            var result = new EmployeeVacationOutput
            {
                ID = Vacation.EmployeeVacationID,
                EmployeeID = Vacation.EmployeeID,
                EmployeeName = Vacation.Employee.EmployeeName,
                VacationTypeID = Vacation.VacationTypeID,
                VacationType = lookups.FirstOrDefault(e => Vacation.VacationTypeID is not null
                                 && e.ID == Vacation.VacationTypeID)?.ColumnDescription,
                FromDate = Vacation.FromDate.IntToDateValue(),
                ToDate = Vacation.ToDate.IntToDateValue()
            };
            return result;
        }

        public async Task<PagedResponse<EmployeeVacationOutput>> GetPage(PaginationFilter<EmployeeVacationFilter> filter)
        {
            if (_userId == -1) throw new UnauthorizedAccessException("Incorrect userId");
            if (!_authService.CheckIfValidUser(_userId)) throw new UnauthorizedAccessException("Incorrect userId");
            int? employeeId = _authService.IsHr(_userId);

            var query = from e in _unitOfWork.EmployeeRepository.PQuery()
                        join lt in _unitOfWork.LookupsRepository.PQuery() on e.DepartmentID equals lt.ID into ltGroup
                        from lt in ltGroup.DefaultIfEmpty()
                        join ev in _unitOfWork.EmployeeVacationRepository.PQuery() on e.EmployeeID equals ev.EmployeeID
                        where (lt.TableName == "Department" && lt.ColumnName == "DepartmentID") && e.ProjectID == _projecId && lt.ProjectID == _projecId && ev.ProjectID == _projecId && (e.EmployeeID == employeeId || lt.EmployeeID == employeeId || employeeId == null)
                        select new EmployeeVacation
                        {
                            Employee = e,
                            EmployeeID = e.EmployeeID,
                            ApprovalStatusID = ev.ApprovalStatusID,
                            EmployeeVacationID = ev.EmployeeVacationID,
                            VacationTypeID = ev.VacationTypeID,
                            ProjectID = ev.ProjectID,
                            FromDate = ev.FromDate,
                            ToDate = ev.ToDate,
                            DayCount=ev.DayCount,
                            Notes=ev.Notes
                        };
            //var query = _unitOfWork.EmployeeVacationRepository.PQuery(include: e => e.Employee);   


            if (filter.FilterCriteria != null)
                query= ApplyFilter(query, filter.FilterCriteria);

            var totalRecords = await query.CountAsync();

            var Vacation = await query.Skip((filter.PageIndex - 1) * filter.Offset)
                    .Take(filter.Offset).ToListAsync();

            var lookups = await _lookupsService.GetLookups(Constants.EmployeeLeaves, Constants.LeaveTypeID);
           
            var approvals = await _lookupsService.GetLookups(Constants.Approvals, string.Empty);

            var result = Vacation.Select(item => new EmployeeVacationOutput 
            {
                ID              = item.EmployeeVacationID,
                EmployeeID      = item.EmployeeID,
                EmployeeName    = item.Employee.EmployeeName,
                VacationTypeID  = item.VacationTypeID,
                VacationType    = lookups.FirstOrDefault(e => item.VacationTypeID is not null
                                 && e.ID == item.VacationTypeID)?.ColumnDescription,
                FromDate        = item.FromDate.IntToDateValue(),
                ToDate          = item.ToDate.IntToDateValue() ,
                DayCount        = item.DayCount,
                Notes           = item.Notes,
                ApprovalStatus  = approvals.FirstOrDefault(e => e.ID == item.ApprovalStatusID)?.ColumnDescription
            }).ToList();

            return result.CreatePagedReponse(filter.PageIndex, filter.Offset, totalRecords);
        }

        private static IQueryable<EmployeeVacation> ApplyFilter(IQueryable<EmployeeVacation> query, EmployeeVacationFilter criteria)
        {
            if (criteria == null)
                return query;

            var parameter = Expression.Parameter(typeof(EmployeeVacation), "e");
            Expression combinedExpression = null;
            if (criteria.EmployeeID != null)
            {
                var employeeIdExpression = Expression.Equal(
                    Expression.Property(parameter, "EmployeeID"),
                    Expression.Constant(criteria.EmployeeID)
                );
                combinedExpression = employeeIdExpression;
            }

            if (criteria.VacationTypeId != null)
            {
                var VacationTypeIdExpression = Expression.Equal(
                    Expression.Property(parameter, "VacationTypeId"),
                    Expression.Constant(criteria.VacationTypeId, typeof(int?))
                );
                combinedExpression = combinedExpression == null
                    ? VacationTypeIdExpression
                    : Expression.AndAlso(combinedExpression, VacationTypeIdExpression);
            }

            if (criteria.FromDate != null )
            {
                var fromDateExpression = Expression.GreaterThanOrEqual(
                    Expression.Property(parameter, "FromDate"),
                    Expression.Constant(criteria.FromDate.DateToIntValue(), typeof(int?))
                );
                combinedExpression = combinedExpression == null
                    ? fromDateExpression
                    : Expression.AndAlso(combinedExpression, fromDateExpression);
            }

            if (criteria.ToDate != null)
            {
                var toDateExpression = Expression.LessThanOrEqual(
                    Expression.Property(parameter, "ToDate"),
                    Expression.Constant(criteria.ToDate.DateToIntValue(), typeof(int?))
                );
                combinedExpression = combinedExpression == null
                    ? toDateExpression
                    : Expression.AndAlso(combinedExpression, toDateExpression);
            }

            if (combinedExpression != null)
            {
                var lambda = Expression.Lambda<Func<EmployeeVacation, bool>>(combinedExpression, parameter);
                query = query.Where(lambda);
            }

            return query;
        }

        public async Task Create(EmployeeVacationInput model)
        {
            if (model == null)
                throw new NotFoundException("recieved data is missed");

            DateTime startDate = (DateTime)model.FromDate;
            DateTime endDate   = (DateTime)model.ToDate;
            TimeSpan dayCount  = endDate.Subtract(startDate);
            int daysDifference = dayCount.Days;
            model.DayCount     = daysDifference;

            var timing = GetVacationTimingInputs(model);

            model.FromDate = null;
            model.ToDate   = null;

            var employeeVacation = _mapper.Map<EmployeeVacationInput, EmployeeVacation>(model);

            employeeVacation.FromDate     = model.FromDate.DateToIntValue();// timing.FromDate;
            employeeVacation.ToDate       = model.ToDate.DateToIntValue();//timing.ToDate;
            //employeeVacation.CreationDate = DateTime.Now;

            await _unitOfWork.EmployeeVacationRepository.PInsertAsync(employeeVacation);

             await _unitOfWork.SaveAsync();
        }

        public async Task Update(EmployeeVacationsUpdate employeeVacation)
        {
            var vacation = _unitOfWork.EmployeeVacationRepository.Get(emp => emp.EmployeeVacationID == employeeVacation.ID)
                .FirstOrDefault();

            if (vacation is null)
                throw new NotFoundException("Data Not Found");

            DateTime startDate        = (DateTime)employeeVacation.FromDate;
            DateTime endDate          = (DateTime)employeeVacation.ToDate;
            TimeSpan dayCount         = endDate.Subtract(startDate);
            vacation.DayCount = dayCount.Days;

            var timing = GetVacationTimingInputs(employeeVacation);

            vacation.FromDate         = employeeVacation.FromDate.DateToIntValue();
            vacation.ToDate           = employeeVacation.FromDate.DateToIntValue();
            vacation.VacationTypeID = employeeVacation.VacationTypeID;
            vacation.Notes = employeeVacation.Notes;

            await _unitOfWork.EmployeeVacationRepository.UpdateAsync(vacation);

            await _unitOfWork.SaveAsync();

        }

        public async Task Delete( int employeeVacationId)
        {
            var Vacation = _unitOfWork.EmployeeVacationRepository
                        .Get(e => e.EmployeeVacationID == employeeVacationId)
                        .FirstOrDefault();

            if (Vacation is null)
                throw new NotFoundException("Data Not Found");

            _unitOfWork.EmployeeVacationRepository.Delete(Vacation);

            await _unitOfWork.SaveAsync();

        }

        private (int? FromDate, int? ToDate) GetVacationTimingInputs(EmployeeVacationInput model)
        {
            return (
                   FromDate: model.FromDate.ConvertFromDateTimeToUnixTimestamp(),
                   ToDate: model.FromDate.ConvertFromDateTimeToUnixTimestamp()
                   );
        }

        private (int? FromDate, int? ToDate) GetVacationTimingInputs(EmployeeVacationsUpdate model)
        {
            return (
                   FromDate: model.FromDate.ConvertFromDateTimeToUnixTimestamp(),
                   ToDate: model.FromDate.ConvertFromDateTimeToUnixTimestamp()
                   );
        }

    }
}
