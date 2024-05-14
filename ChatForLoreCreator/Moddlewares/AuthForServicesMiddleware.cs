namespace ChatForLoreCreator.Moddlewares;

public class AuthForServicesMiddleware
{
    private readonly RequestDelegate _requestDelegate = null!;

    public AuthForServicesMiddleware(RequestDelegate requestDelegate)
    {
        _requestDelegate = requestDelegate;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if(!context.Request.Query.ToString().StartsWith("/chating"))
        {
            await _requestDelegate.Invoke(context);
            return;
        }
        var authToken = context.Request.Headers["authsmile"];
        if(string.IsNullOrEmpty(authToken) || !authToken.Any())
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync(" BYE BYE ");
            return;
        }

        if(authToken != "123")
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsync("Access forbidden");
            return;
        }
        await _requestDelegate.Invoke(context);
    }
}
