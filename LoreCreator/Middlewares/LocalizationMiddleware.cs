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
        string locale = context.Request.Cookies.FirstOrDefault(x => x.Key == "loreCreatorLanguage").Value;
        if(string.IsNullOrEmpty(locale))
        {
            string acceptLanguage = context.Request.Headers.AcceptLanguage;
            locale = acceptLanguage.Split(';').First().Split(',').First();
        }

        var culture = new System.Globalization.CultureInfo(locale);
        Thread.CurrentThread.CurrentCulture = culture;
        Thread.CurrentThread.CurrentUICulture = culture;

        await next.Invoke(context);
    }
}
