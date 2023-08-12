using AutoMapper;
using BusinessLogicLayer.Common;
using BusinessLogicLayer.Exceptions;
using BusinessLogicLayer.Extensions;
using BusinessLogicLayer.Services.Lookups;
using BusinessLogicLayer.UnitOfWork;
using DataAccessLayer.DTO;
using DataAccessLayer.DTO.EmployeeVacations;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogicLayer.Services.EmployeeVacations
{
    internal class EmployeeVacationService : IEmployeeVacationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILookupsService _lookupsService;
        private readonly IMapper _mapper;
        public EmployeeVacationService(IUnitOfWork unityOfWork, ILookupsService lookupsService, IMapper mapper)
        {
            _unitOfWork     = unityOfWork;
            _lookupsService = lookupsService;
            _mapper         = mapper;
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
                FromDate = Vacation.FromDate.ConvertFromUnixTimestampToDateTime(),
                ToDate = Vacation.ToDate.ConvertFromUnixTimestampToDateTime()
            };
            return result;
        }

        public async Task<PagedResponse<EmployeeVacationOutput>> GetPage(PaginationFilter<EmployeeVacationFilter> filter)
        {
            var query = _unitOfWork.EmployeeVacationRepository.PQuery(include: e => e.Employee);   

            var totalRecords = await query.CountAsync();

            if (filter.FilterCriteria != null)
                ApplyFilter(query, filter.FilterCriteria);


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
                FromDate        = item.FromDate.ConvertFromUnixTimestampToDateTime(),
                ToDate          = item.ToDate.ConvertFromUnixTimestampToDateTime() ,
                DayCount        = item.DayCount,
                Notes           = item.Notes,
                ApprovalStatus  = approvals.FirstOrDefault(e => e.ID == item.ApprovalStatusID)?.ColumnDescription
            }).ToList();

            return result.CreatePagedReponse(filter.PageIndex, filter.Offset, totalRecords);
        }

        private static IQueryable<EmployeeVacation> ApplyFilter(IQueryable<EmployeeVacation> query, EmployeeVacationFilter criteria)
        {
            if (criteria.EmployeeID != null)
                query = query.Where(e => e.EmployeeID == criteria.EmployeeID);

            if (criteria.FromDate != null)
                query = query.Where(e => e.FromDate == criteria.FromDate.DateToIntValue());

            if (criteria.ToDate != null)
                query = query.Where(e => e.ToDate == criteria.ToDate.DateToIntValue());

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
