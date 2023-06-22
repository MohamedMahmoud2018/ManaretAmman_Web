using BusinessLogicLayer.Repositories;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly PayrolLogOnlyContext context;

        public UnitOfWork(PayrolLogOnlyContext _context)
        {
            context = _context;
        }


        private IRepository<EmployeeLeaf> employeeLeafRepo;
        public IRepository<EmployeeLeaf> EmployeeLeafRepo
        {
            get { return employeeLeafRepo ?? (employeeLeafRepo = new Repository<EmployeeLeaf>(context)); }
        }



        public void Save()
        {
            context.SaveChanges();
        }
        public void Dispose()
        {
            context.Dispose();
            System.GC.SuppressFinalize(this);
        }
    }
}
