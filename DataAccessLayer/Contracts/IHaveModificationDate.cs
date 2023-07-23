using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Contracts
{
    public interface IHaveModificationDate
    {
        public int? ModifiedBy { get; set; }
        public DateTime? ModificationDate { get; set; }
    }
}
