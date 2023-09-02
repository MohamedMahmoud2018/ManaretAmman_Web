using DataAccessLayer.Contracts;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Models
{
    public class BaseEntity : IBaseEntity
    {
        public int? CreatedBy { get ;  set ;  }

        [Column(TypeName = "datetime")]
        public DateTime? CreationDate { get ;  set ;  }

        public int? ModifiedBy { get ;  set ;  }

        [Column(TypeName = "datetime")]
        public DateTime? ModificationDate { get ;  set ;  }
    }
}
