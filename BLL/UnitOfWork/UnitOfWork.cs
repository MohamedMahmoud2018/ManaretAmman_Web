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

        private IRepository<EmployeeLeaf> _employeeLeaveRepository;

        private IRepository<LookupTable> _lookupsRepository;



        public IRepository<Employee> EmployeeRepository
        {
            get { return _employeeRepository ?? (_employeeRepository = new Repository<Employee>(_context, _projectProvider)); }
        } 

        public IRepository<EmployeeLeaf> EmployeeLeaveRepository
        {
            get { return _employeeLeaveRepository ?? (_employeeLeaveRepository = new Repository<EmployeeLeaf>(_context, _projectProvider)); }
        }

        public IRepository<LookupTable> LookupsRepository
        {
            get { return _lookupsRepository ?? (_lookupsRepository = new Repository<LookupTable>(_context, _projectProvider)); }
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
