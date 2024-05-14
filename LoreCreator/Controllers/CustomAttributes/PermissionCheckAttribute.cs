using LoreCreator.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SharedForLoreCreator.Models;

namespace LoreCreator.Controllers.CustomAttributes;

public class PermissionCheckAttribute : Attribute, IAuthorizationFilter
{
    private List<PermissionTypes> PermissionTypes { get; init; } = new();

    public PermissionCheckAttribute(params PermissionTypes[] permissionTypes)
    {
        PermissionTypes.AddRange(permissionTypes);
    }
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        AuthService authService = context.HttpContext.RequestServices.GetService<AuthService>();
        foreach (PermissionTypes permissionType in PermissionTypes)
        {
            if (authService.IsUserHavePermissionAsync(permissionType).Result)
            {
                context.Result = new ForbidResult(AuthController.AUTH_KEY);
            }
        }
    }
}
