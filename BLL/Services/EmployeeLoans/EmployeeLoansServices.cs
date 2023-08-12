using AutoMapper;
using BusinessLogicLayer.Common;
using BusinessLogicLayer.Exceptions;
using BusinessLogicLayer.Extensions;
using BusinessLogicLayer.Services.Lookups;
using BusinessLogicLayer.UnitOfWork;
using DataAccessLayer.DTO;
using DataAccessLayer.DTO.EmployeeLoans;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogicLayer.Services.EmployeeLoans
{
    internal class EmployeeLoansService : IEmployeeLoansService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILookupsService _lookupsService;
        private readonly IMapper _mapper;
        public EmployeeLoansService(IUnitOfWork unityOfWork, ILookupsService lookupsService, IMapper mapper)
        {
            _unitOfWork     = unityOfWork;
            _lookupsService = lookupsService;
            _mapper         = mapper;
        }
        public async Task<EmployeeLoansOutput> Get(int id)
        {
            var Loan = _unitOfWork.EmployeeLoanRepository
                       .PQuery(e => e.EmployeeLoanID == id, include: e => e.Employee)
                       .FirstOrDefault();

            if (Loan is null)
                throw new NotFoundException("data not found");


            var result = new EmployeeLoansOutput
            {
                ID = Loan.EmployeeLoanID,
                EmployeeID = Loan.Employee.EmployeeID,
                EmployeeName = Loan.Employee.EmployeeName,
                LoanDate = Loan.LoanDate.IntToDateValue(),
                LoanAmount = Loan.LoanAmount,
                ProjectID = Loan.ProjectID,
                LoantypeId=Loan.loantypeid,
                loantypeAr = Loan.loantypeid is not null ? Constants.GetEmployeeLoanDictionary[Loan.loantypeid.Value].NameAr : null,
                loantypeEn = Loan.loantypeid is not null ? Constants.GetEmployeeLoanDictionary[Loan.loantypeid.Value].NameEn : null
            };

            return result;
        }

        public async Task<PagedResponse<EmployeeLoansOutput>> GetPage(PaginationFilter<EmployeeLoanFilter> filter)
        {
            var query = _unitOfWork.EmployeeLoanRepository.PQuery(include: e => e.Employee);

            var totalRecords = await query.CountAsync();

            if (filter.FilterCriteria != null)
                ApplyFilter(query, filter.FilterCriteria);


            var Loans = await query.Skip((filter.PageIndex - 1) * filter.Offset)
                        .Take(filter.Offset).ToListAsync();

            //var lookups = await _lookupsService.GetLookups(Constants.EmployeeLoans, Constants.LoanTypeID);
            var approvals = await _lookupsService.GetLookups(Constants.Approvals, string.Empty);

            var result = Loans.Select(item => new EmployeeLoansOutput
            {
                ID             = item.EmployeeLoanID,
                EmployeeID     = item.Employee.EmployeeID,
                EmployeeName   = item.Employee.EmployeeName,
                LoanDate       = item.LoanDate.IntToDateValue(),
                LoanAmount     = item.LoanAmount  ,
                ProjectID = item.ProjectID,
                LoantypeId = item.loantypeid,
                loantypeAr = item.loantypeid is not null? Constants.GetEmployeeLoanDictionary[item.loantypeid.Value].NameAr:null,
                loantypeEn = item.loantypeid is not null ? Constants.GetEmployeeLoanDictionary[item.loantypeid.Value].NameEn : null,
                ApprovalStatus = approvals.FirstOrDefault(e => e.ColumnValue == item.ApprovalStatusID.ToString())?.ColumnDescription
            });

            return result.CreatePagedReponse(filter.PageIndex, filter.Offset, totalRecords);
        }

        private static IQueryable<EmployeeLoan> ApplyFilter(IQueryable<EmployeeLoan>  query, EmployeeLoanFilter criteria)
        {
            if (criteria.EmployeeID != null)
                query = query.Where(e => e.EmployeeID == criteria.EmployeeID);

            if (criteria.LoanDate != null)
                query = query.Where(e => e.LoanDate == criteria.LoanDate.ConvertFromDateTimeToUnixTimestamp());

            if (criteria.LoanTypeId != null)
                query = query.Where(e => e.loantypeid == criteria.LoanTypeId);

            return query; 
        }

        public async Task Create(EmployeeLoansInput model)
        {
            if (model == null)
                throw new NotFoundException("recieved data is missed");

            var LoanDate = model.LoanDate.DateToIntValue();

            model.LoanDate = null;

            var employeeLoan = _mapper.Map<EmployeeLoan>(model);

            employeeLoan.LoanDate    = LoanDate;

            await _unitOfWork.EmployeeLoanRepository.PInsertAsync(employeeLoan);

             await _unitOfWork.SaveAsync();
        }

        public async Task Update(EmployeeLoansUpdate employeeLoan)
        {
            var Loan = _unitOfWork.EmployeeLoanRepository.Get(emp => emp.EmployeeLoanID == employeeLoan.ID)
                .FirstOrDefault();

            if (Loan is null)
                throw new NotFoundException("Data Not Found");

            Loan.LoanDate = employeeLoan.LoanDate.DateToIntValue();
            Loan.LoanAmount = employeeLoan.LoanAmount;
            Loan.Notes = employeeLoan.Notes;

            await _unitOfWork.EmployeeLoanRepository.UpdateAsync(Loan);

            await _unitOfWork.SaveAsync();

        }

        public async Task Delete( int employeeLoanId)
        {
            var Loan = _unitOfWork.EmployeeLoanRepository
                        .Get(e => e.EmployeeLoanID == employeeLoanId)
                        .FirstOrDefault();

            if (Loan is null)
                throw new NotFoundException("Data Not Found");

            _unitOfWork.EmployeeLoanRepository.Delete(Loan);

            await _unitOfWork.SaveAsync();

        }

        private  int? GetLoanTimingInputs(DateTime? LoanDate)
        {
            return LoanDate.ConvertFromDateTimeToUnixTimestamp();
               
        }
    }
}
