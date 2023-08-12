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

            // var lookups = await _lookupsService.GetLookups(Constants.EmployeeLoans, Constants.LoanTypeID);

            var result = new EmployeeLoansOutput
            {
                ID = Loan.EmployeeLoanID,
                EmployeeID = Loan.EmployeeID,
                EmployeeName = Loan.Employee.EmployeeName,
                //loantypeid = Loan.loantypeid,
                //loantypeEn = Constants.GetEmployeeLoanDictionary[Loan.loantypeid.Value].NameEn,
                //loantypeAr = Constants.GetEmployeeLoanDictionary[Loan.loantypeid.Value].NameAr,
                LoanDate = Loan.LoanDate.ConvertFromUnixTimestampToDateTime(),
                LoanAmount = Loan.LoanAmount
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
                ID = item.EmployeeLoanID,
                EmployeeID = item.EmployeeID,
                EmployeeName = item.Employee.EmployeeName,
                //loantypeid     = item.loantypeid,
                //loantypeEn = Constants.GetEmployeeLoanDictionary[item.loantypeid.Value].NameEn,
                //loantypeAr = Constants.GetEmployeeLoanDictionary[item.loantypeid.Value].NameAr,
                LoanDate = item.LoanDate.ConvertFromUnixTimestampToDateTime(),
                LoanAmount = item.LoanAmount,
                ApprovalStatus = approvals.FirstOrDefault(e => e.ID == item.ApprovalStatusID)?.ColumnDescription
            }).ToList();

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

            var LoanDate = GetLoanTimingInputs(model.LoanDate);

            model.LoanDate = null;

            var employeeLoan = _mapper.Map<EmployeeLoan>(model);

            employeeLoan.LoanDate    = LoanDate;
            //employeeLoan.CreationDate = DateTime.Now;

            await _unitOfWork.EmployeeLoanRepository.PInsertAsync(employeeLoan);

             await _unitOfWork.SaveAsync();
        }

        public async Task Update(EmployeeLoansUpdate employeeLoan)
        {
            var Loan = _unitOfWork.EmployeeLoanRepository.Get(emp => emp.EmployeeLoanID == employeeLoan.ID)
                .FirstOrDefault();

            if (Loan is null)
                throw new NotFoundException("Data Not Found");

            var timing = GetLoanTimingInputs(employeeLoan.LoanDate);

            employeeLoan.LoanDate = null;

            var updatedLoan = _mapper.Map<EmployeeLoansUpdate, EmployeeLoan>(employeeLoan);

            updatedLoan.LoanDate = timing;

            await _unitOfWork.EmployeeLoanRepository.UpdateAsync(updatedLoan);

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
