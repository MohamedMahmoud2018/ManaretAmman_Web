using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        //private readonly ApplicationDBContext context;

        //public UnitOfWork(ApplicationDBContext _context)
        //{
        //    context = _context;
        //}


        //private IRepository<Gender> genderRepo;
        //public IRepository<Gender> GenderRepo
        //{
        //    get { return genderRepo ?? (genderRepo = new Repository<Gender>(context)); }
        //}



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
