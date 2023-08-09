using BusinessLogicLayer.Services.Balance;
using BusinessLogicLayer.Services.EmployeeLeaves;
using BusinessLogicLayer.Services.Notification;
using DataAccessLayer.DTO;
using DataAccessLayer.DTO.Notification;
using DataAccessLayer.Models;
using ManaretAmman.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ManaretAmman.Controllers.Employees
{
    [Route("api/Employees/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationsService _notificationService;

        public NotificationController(INotificationsService notificationService)
        => _notificationService = notificationService;

        [HttpPost("GetNotification")]
        public async Task<IApiResponse> GetNotification(GetEmployeeNotificationInput model)
        {
            var result = await _notificationService.GetRemindersAsync(model);
            if (result == null || result.Count == 0)
            {
                List<GetRemindersResult> res = new List<GetRemindersResult>();
                res.Add(new GetRemindersResult());
                return ApiResponse<List<GetRemindersResult>>.Failure(res, null);
            }
            return ApiResponse<List<GetRemindersResult>>.Success(result);
        }
    }
}
