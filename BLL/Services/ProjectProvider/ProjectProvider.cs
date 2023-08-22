using Microsoft.AspNetCore.Http;

namespace BusinessLogicLayer.Services.ProjectProvider;

public class ProjectProvider : IProjectProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ProjectProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    public int GetProjectId()
    {
        var projectId = _httpContextAccessor.HttpContext.Request.Headers["ProjectId"].ToString();

        return int.Parse(projectId);
    }
    public int UserId()
    {
        string userId = _httpContextAccessor.HttpContext.Request.Headers["UserId"].ToString();
        if (string.IsNullOrEmpty(userId)) return -1;
        return int.Parse(userId);
    }
}
