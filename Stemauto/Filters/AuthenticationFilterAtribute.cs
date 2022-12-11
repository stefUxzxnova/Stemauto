using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Stemauto.Extentions;
using Stemauto.Entities;

namespace Stemauto.ActionFilters
{
    public class AuthenticationFilterAtribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Session.GetObject<User>("loggedUser") == null)
            {
                context.Result = new RedirectResult("/Home/Login");

            }
        }
    }
}
