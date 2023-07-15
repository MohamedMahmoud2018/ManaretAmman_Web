using BusinessLogicLayer.Repositories;
using DataAccessLayer.Models;

namespace BusinessLogicLayer.UnitOfWork
{
    public interface IUnitOfWork
    {
        IRepository<Employee> EmployeeRepository { get; }
        IRepository<LookupTable> LookupsRepository { get; }

        IRepository<EmployeeLeaf> EmployeeLeaveRepository { get; }
        IRepository<EmployeeVacation> EmployeeVacationRepository { get; }

        void Save();
        Task SaveAsync();
    }
}
