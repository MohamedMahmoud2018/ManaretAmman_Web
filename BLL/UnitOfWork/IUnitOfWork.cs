using BusinessLogicLayer.Repositories;
using DataAccessLayer.DTO;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.UnitOfWork
{
    interface IUnitOfWork
    {
        IRepository<EmployeeLeaf> EmployeeLeafRepo { get; }

        void Save();
    }
}
