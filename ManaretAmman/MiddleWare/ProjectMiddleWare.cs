namespace ManaretAmman.MiddleWare
{
    public class ProjectMiddleware
    {
        private readonly RequestDelegate _next;

        public ProjectMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Headers.TryGetValue("ProjectId", out var ProjectIdValue))
            {
                context.Items["ProjectId"] = ProjectIdValue.ToString();
            }
            else
            {
                // Handle the case when projectId is not provided
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("ProjectId is missing from the request header.");
                return;
            }

            await _next(context);
        }
    }
}