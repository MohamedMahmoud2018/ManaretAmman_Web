using AutoMapper;
using BusinessLogicLayer.Extensions;
using BusinessLogicLayer.UnitOfWork;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services.Notification
{
    public class NotificationsService : INotificationsService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;
        private readonly PayrolLogOnlyContext _payrolLogOnlyContext;
        public NotificationsService(IUnitOfWork unit, IMapper mapper, PayrolLogOnlyContext payrolLogOnlyContext)
        {
            _unit = unit;
            _mapper = mapper;
            _payrolLogOnlyContext = payrolLogOnlyContext;
        }
        public async Task<List<GetRemindersResult>> GetRemindersAsync(int projectId, int userId,DateTime? fromdate,DateTime? toDate)
        {
            var result = await _payrolLogOnlyContext.GetProcedures().GetRemindersAsync(projectId, null, 1, 0, fromdate.DateToIntValue(), toDate.DateToIntValue(), null, userId, null);
            return result;
        }
    }
}
