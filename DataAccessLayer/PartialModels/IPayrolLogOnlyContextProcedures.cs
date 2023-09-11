using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    //custom for notifications;
    public partial interface IPayrolLogOnlyContextProcedures
    {
        Task<int?> ChangeEmployeeRequestStatusAsync(int? pEmployeeID, int? pCreatedBy, int? pApprovalStatusID, int? pApprovalPageID, int? pProjectID, int? pID, int? pPrevilageType, int? pSendToLog, int? pPK,bool custom, OutputParameter<int?> pError, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
    }
}
