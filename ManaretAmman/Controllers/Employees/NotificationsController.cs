using BusinessLogicLayer.Services.Notification;
using DataAccessLayer.DTO.Notification;
using DataAccessLayer.Models;
using ManaretAmman.Models;
using Microsoft.AspNetCore.Mvc;

namespace ManaretAmman.Controllers.Employees
{
    [Route("api/Employees/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationsService _notificationService;

        public NotificationsController(INotificationsService notificationService)
        => _notificationService = notificationService;

        [HttpPost("GetNotifications")]
        public async Task<IApiResponse> GetNotifications(GetEmployeeNotificationInput model)
        {
            var result = await _notificationService.GetNotificationsAsync(model);

            if (result == null || result.Count == 0)
            {
                List<RemiderOutput> res = new List<RemiderOutput>();

                res.Add(new RemiderOutput());

                return ApiResponse<List<RemiderOutput>>.Failure(res, null);
            }

            return ApiResponse<List<RemiderOutput>>.Success(result);
        }

        [HttpPost("AcceptOrRejectNotifications")]
        public async Task<IApiResponse> AcceptOrRejectNotifications(AcceptOrRejectNotifcationInput model)
        {
            var result = await _notificationService.AcceptOrRejectNotificationsAsync(model);
            if (result == null || result.Count == 0)
            {
                List<ChangeEmployeeRequestStatusResult> res = new List<ChangeEmployeeRequestStatusResult>();
                res.Add(new ChangeEmployeeRequestStatusResult());
                return ApiResponse<List<ChangeEmployeeRequestStatusResult>>.Failure(res, null);
            }
            return ApiResponse<List<ChangeEmployeeRequestStatusResult>>.Success(result);
        }
    }
}
