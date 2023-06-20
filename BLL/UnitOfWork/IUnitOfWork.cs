using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.UnitOfWork
{
    interface IUnitOfWork
    {
        //IRepository<Gender> GenderRepo { get; }

        void Save();
    }
}
