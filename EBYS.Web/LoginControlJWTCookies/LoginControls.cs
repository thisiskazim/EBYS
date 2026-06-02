using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EBYS.Web.LoginControlJWTCookies
{
    public class LoginControls : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
           
            var controllerName = context.RouteData.Values["controller"]?.ToString();
            if (controllerName == "Login") return;

      
            var token = context.HttpContext.Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token))
            { 
                context.Result = new RedirectToActionResult("Login", "Login", null);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
