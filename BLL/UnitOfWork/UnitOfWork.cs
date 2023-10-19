using BusinessLogicLayer.Exceptions;
using BusinessLogicLayer.Repositories;
using BusinessLogicLayer.Services.ProjectProvider;
using DataAccessLayer.Models;

namespace BusinessLogicLayer.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly PayrolLogOnlyContext _context;
        private readonly IProjectProvider _projectProvider;

        public UnitOfWork(PayrolLogOnlyContext context, IProjectProvider projectProvider)
        {
            this._context    = context;
            _projectProvider = projectProvider;
        }



        private IRepository<Employee> _employeeRepository;

        public IRepository<Employee> EmployeeRepository
        {
            get { return _employeeRepository ?? (_employeeRepository = new Repository<Employee>(_context, _projectProvider)); }
        } 

       
        private IRepository<LookupTable> _lookupsRepository;

        public IRepository<LookupTable> LookupsRepository
        {
            get { return _lookupsRepository ?? (_lookupsRepository = new Repository<LookupTable>(_context, _projectProvider)); }
        }
        
        private IRepository<EmployeeLeaf> _employeeLeaveRepository;

        public IRepository<EmployeeLeaf> EmployeeLeaveRepository
        {
            get { return _employeeLeaveRepository ?? (_employeeLeaveRepository = new Repository<EmployeeLeaf>(_context, _projectProvider)); }
        }

         private IRepository<EmployeeVacation> _employeeVacationRepository;

        public IRepository<EmployeeVacation> EmployeeVacationRepository
        {
            get { return _employeeVacationRepository ?? (_employeeVacationRepository = new Repository<EmployeeVacation>(_context, _projectProvider)); }
        }

        private IRepository<EmployeeLoan> _employeeLoanRepository;

        public IRepository<EmployeeLoan> EmployeeLoanRepository
        {
            get { return _employeeLoanRepository ?? (_employeeLoanRepository = new Repository<EmployeeLoan>(_context, _projectProvider)); }
        }
        private IRepository<GetEmployeeBalanceReportResult> _employeeBalanceRepository;

        public IRepository<GetEmployeeBalanceReportResult> EmployeeBalanceRepository {
            get { return _employeeBalanceRepository ?? (_employeeBalanceRepository = new Repository<GetEmployeeBalanceReportResult>(_context, _projectProvider)); }
        }

        IRepository<User> _userRepository;
        public IRepository<User> UserRepository
        {
            get { return _userRepository ?? (_userRepository = new Repository<User>(_context, _projectProvider)); }
        } 
        
        IRepository<Project> _projectRepository;
        public IRepository<Project> ProjectRepository
        {
            get { return _projectRepository ?? (_projectRepository = new Repository<Project>(_context)); }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
        public void Dispose()
        {
            _context.Dispose();
            System.GC.SuppressFinalize(this);
        }
    }
}
