using BusinessLogicLayer.Repositories;
using DataAccessLayer.Models;

namespace BusinessLogicLayer.UnitOfWork
{
    public interface IUnitOfWork
    {
        IRepository<EmployeeLeaf> EmployeeLeaveRepository { get; }
        IRepository<LookupTable> LookupsRepository { get; }

        void Save();
        Task SaveAsync();
    }
}
