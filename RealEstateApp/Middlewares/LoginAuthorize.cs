using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RealEstateApp.Core.Application.Enums;
using RealEstateApp.Middlewares;

namespace WebApp.RealEstateApp.Middlewares
{
    public class LoginAuthorize : IAsyncActionFilter
    {
        private readonly ValidateUserSession _userSession;

        public LoginAuthorize(ValidateUserSession userSession)
        {
            _userSession = userSession;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (_userSession.HasUser())
            {
                var userViewModel = _userSession.GetUser();

                if (userViewModel.Roles.Any(role => role == Roles.Admin.ToString() || role == Roles.Client.ToString() || role == Roles.Agent.ToString()))
                {
                    var controller = context.Controller as ControllerBase;
                    context.Result = controller.RedirectToAction("AccessDenied", "User");
                    return;
                }
            }

            await next();
        }
    }
}
