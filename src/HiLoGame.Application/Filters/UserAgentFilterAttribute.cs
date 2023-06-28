using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HiLoGame.Application.Filters;

public class UserAgentFilterAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var userAgent = context.HttpContext.Request.Headers["User-Agent"];

        if (string.IsNullOrEmpty(userAgent))
        {
            context.Result = new BadRequestObjectResult("User-Agent header is missing");
            return;
        }
    }
}
