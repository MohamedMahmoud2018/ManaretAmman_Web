using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public partial class PayrolLogOnlyContextProcedures
    {
        //custom for notifications;

        public virtual async Task<int?> ChangeEmployeeRequestStatusAsync(int? pEmployeeID, int? pCreatedBy, int? pApprovalStatusID, int? pApprovalPageID, int? pProjectID, int? pID, int? pPrevilageType, int? pSendToLog, int? pPK, bool custom, OutputParameter<int?> pError, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterpError = new SqlParameter
            {
                ParameterName = "pError",
                Direction = System.Data.ParameterDirection.InputOutput,
                Value = pError?._value ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.Int,
            };
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new[]
            {
                new SqlParameter
                {
                    ParameterName = "pEmployeeID",
                    Value = pEmployeeID ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                new SqlParameter
                {
                    ParameterName = "pCreatedBy",
                    Value = pCreatedBy ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                new SqlParameter
                {
                    ParameterName = "pApprovalStatusID",
                    Value = pApprovalStatusID ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                new SqlParameter
                {
                    ParameterName = "pApprovalPageID",
                    Value = pApprovalPageID ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                new SqlParameter
                {
                    ParameterName = "pProjectID",
                    Value = pProjectID ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                new SqlParameter
                {
                    ParameterName = "pID",
                    Value = pID ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                new SqlParameter
                {
                    ParameterName = "pPrevilageType",
                    Value = pPrevilageType ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                parameterpError,
                new SqlParameter
                {
                    ParameterName = "pSendToLog",
                    Value = pSendToLog ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                new SqlParameter
                {
                    ParameterName = "pPK",
                    Value = pPK ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<ChangeEmployeeRequestStatusResult>("EXEC @returnValue = [dbo].[ChangeEmployeeRequestStatus] @pEmployeeID, @pCreatedBy, @pApprovalStatusID, @pApprovalPageID, @pProjectID, @pID, @pPrevilageType, @pError OUTPUT, @pSendToLog, @pPK", sqlParameters, cancellationToken);

            pError?.SetValue(parameterpError.Value);
            returnValue?.SetValue(parameterreturnValue.Value);

            return (int?)parameterpError.Value;
        }

    }
}
