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
}
