using AutoMapper;
using BusinessLogicLayer.Common;
using BusinessLogicLayer.Exceptions;
using BusinessLogicLayer.Extensions;
using BusinessLogicLayer.Services.Lookups;
using BusinessLogicLayer.UnitOfWork;
using DataAccessLayer.DTO;
using DataAccessLayer.Models;

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
                loantypeid = Loan.loantypeid,
                loantypeEn = Constants.EmployeeLoanDictionary[Loan.EmployeeLoanID].NameEn,
                loantypeAr = Constants.EmployeeLoanDictionary[Loan.EmployeeLoanID].NameAr,
                LoanDate = Loan.LoanDate.ConvertFromUnixTimestampToDateTime(),
                LoanAmount = Loan.LoanAmount
            };

            return result;
        }

        public async Task<List<EmployeeLoansOutput>> GetAll()
        {
            var Loans = _unitOfWork.EmployeeLoanRepository.PQuery(include: e => e.Employee).ToList();

            //var lookups = await _lookupsService.GetLookups(Constants.EmployeeLoans, Constants.LoanTypeID);

            var result = Loans.Select(item => new EmployeeLoansOutput 
            {
                ID = item.EmployeeLoanID,
                EmployeeID      = item.EmployeeID,
                EmployeeName    = item.Employee.EmployeeName,
                loantypeid     = item.loantypeid,
                loantypeEn = Constants.EmployeeLoanDictionary[item.EmployeeLoanID].NameEn,
                loantypeAr = Constants.EmployeeLoanDictionary[item.EmployeeLoanID].NameAr,
                LoanDate = item.LoanDate.ConvertFromUnixTimestampToDateTime(),
                LoanAmount = item.LoanAmount
            });

            return result.ToList();
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

        public async Task Update(EmployeeLoansInput employeeLoan)
        {
            var Loan = _unitOfWork.EmployeeLoanRepository.Get(emp => emp.EmployeeLoanID == employeeLoan.ID)
                .FirstOrDefault();

            if (Loan is null)
                throw new NotFoundException("Data Not Found");

            var timing = GetLoanTimingInputs(employeeLoan.LoanDate);

            employeeLoan.LoanDate = null;

            var updatedLoan = _mapper.Map<EmployeeLoansInput, EmployeeLoan>(employeeLoan);

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
