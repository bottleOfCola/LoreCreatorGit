using System.Security;

namespace LoreCreator.Middlewares;

public class LocalizationMiddleware
{
    private readonly RequestDelegate next;

    public LocalizationMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        string acceptLanguage = context.Request.Headers.AcceptLanguage;
        var locale = acceptLanguage.Split(';').First().Split(',').First();

        var culture = new System.Globalization.CultureInfo(locale);
        Thread.CurrentThread.CurrentCulture = culture;
        Thread.CurrentThread.CurrentUICulture = culture;

        await next.Invoke(context);
    }
}
