using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EBYS.Web.LoginControlJWTCookies
{
    public class LoginControls : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // 1. Zaten Auth (Login) sayfasındaysak kontrol etme (Sonsuz döngü olmasın)
            var controllerName = context.RouteData.Values["controller"]?.ToString();
            if (controllerName == "Login") return;

            // 2. Cookie'de bilet var mı bak
            var token = context.HttpContext.Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token))
            {
                // Bilet yoksa direkt Login'e şutla
                context.Result = new RedirectToActionResult("Login", "Login", null);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
