using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;

namespace Infrastructure.Identity;
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class ClaimAttribute : Attribute, IAuthorizationFilter
{
    private string Permission;
    public ClaimAttribute(string permission)
    {
        Permission = permission;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        string permissions = (string)context.HttpContext.Items["Permissions"];
        if (permissions == null)
        {
            context.Result = new JsonResult(new { message = "Permission missing" }) { StatusCode = StatusCodes.Status401Unauthorized };
        }
        var permissionList = JsonSerializer.Deserialize<string[]>(permissions);

        if (!permissionList.Contains(Permission))
        {
            context.Result = new JsonResult(new { message = "Permission missing" }) { StatusCode = StatusCodes.Status401Unauthorized };
        }
    }
}