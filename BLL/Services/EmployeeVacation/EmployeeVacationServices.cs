using AutoMapper;
using BusinessLogicLayer.Common;
using BusinessLogicLayer.Exceptions;
using BusinessLogicLayer.Extensions;
using BusinessLogicLayer.Services.Lookups;
using BusinessLogicLayer.UnitOfWork;
using DataAccessLayer.DTO;
using DataAccessLayer.Models;

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
                                 && e.ColumnValue == Vacation.VacationTypeID.ToString())?.ColumnDescription,
                FromDate = Vacation.FromDate.ConvertFromUnixTimestampToDateTime(),
                ToDate = Vacation.ToDate.ConvertFromUnixTimestampToDateTime()
            };
            return result;
        }

        public async Task<List<EmployeeVacationOutput>> GetAll()
        {
            var Vacation = _unitOfWork.EmployeeVacationRepository.PQuery(include: e => e.Employee).ToList();

            var lookups = await _lookupsService.GetLookups(Constants.VacationType, Constants.VacationTypeId);

            var result = Vacation.Select(item => new EmployeeVacationOutput 
            {
                ID = item.EmployeeVacationID,
                EmployeeID      = item.EmployeeID,
                EmployeeName    = item.Employee.EmployeeName,
                VacationTypeID     = item.VacationTypeID,
                VacationType       = lookups.FirstOrDefault(e => item.VacationTypeID is not null
                                 && e.ColumnValue == item.VacationTypeID.ToString())?.ColumnDescription,
                FromDate = item.FromDate.ConvertFromUnixTimestampToDateTime(),
                ToDate = item.ToDate.ConvertFromUnixTimestampToDateTime()
            });

            return result.ToList();
        }

        public async Task Create(EmployeeVacationInput model)
        {
            if (model == null)
                throw new NotFoundException("recieved data is missed");

            DateTime startDate = (DateTime)model.FromDate;
            DateTime endDate = (DateTime)model.ToDate;
            TimeSpan dayCount = endDate.Subtract(startDate);
            int daysDifference = dayCount.Days;
            model.DayCount= daysDifference;
            //var timing = GetVacationTimingInputs(model);

            //model.VacationDate = null;
            //model.FromTime  = null;
            //model.ToTime    = null;

            var employeeVacation = _mapper.Map<EmployeeVacation>(model);

            employeeVacation.FromDate = Constants.ConvertFromDateFormat(1,model.FromDate);
            employeeVacation.ToDate = Constants.ConvertFromDateFormat(1,model.ToDate);

            employeeVacation.VacationTypeID = null;


            await _unitOfWork.EmployeeVacationRepository.PInsertAsync(employeeVacation);

             await _unitOfWork.SaveAsync();
        }

        public async Task Update(EmployeeVacationInput employeeVacation)
        {
            var Vacation = _unitOfWork.EmployeeVacationRepository.Get(emp => emp.EmployeeVacationID == employeeVacation.ID)
                .FirstOrDefault();

            if (Vacation is null)
                throw new NotFoundException("Data Not Found");

            DateTime startDate = (DateTime)employeeVacation.FromDate;
            DateTime endDate = (DateTime)employeeVacation.ToDate;
            TimeSpan dayCount = endDate.Subtract(startDate);
            int daysDifference = dayCount.Days;
            employeeVacation.DayCount = daysDifference;

            //var timing = GetVacationTimingInputs(employeeVacation);

            //employeeVacation.VacationDate = null;
            //employeeVacation.FromTime  = null;
            //employeeVacation.ToTime    = null;

            var updatedVacation = _mapper.Map<EmployeeVacationInput, EmployeeVacation>(employeeVacation);

            updatedVacation.FromDate = Constants.ConvertFromDateFormat(1, employeeVacation.FromDate);
            updatedVacation.ToDate = Constants.ConvertFromDateFormat(1, employeeVacation.ToDate);

            await _unitOfWork.EmployeeVacationRepository.UpdateAsync(updatedVacation);

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

        //private (int? FromTime, int? ToTime, int? VacationDate) GetVacationTimingInputs(EmployeeVacationInput model)
        //{
        //    return (
        //           FromTime: model.FromTime.ConvertFromTimeStringToMinutes() ,
        //           ToTime: model.ToTime.ConvertFromTimeStringToMinutes(),
        //           VacationDate: model.VacationDate.ConvertFromDateTimeToUnixTimestamp()
        //        ) ;
        //}

    }
}
