using Microsoft.AspNetCore.Mvc.Filters;
using Fiap.CloudGames.Fase1.Infrastructure.Helpers;

namespace Fiap.CloudGames.Fase1.Infrastructure.Middleware.CustomException
{
    public class CustomExceptionMiddleware : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            string controllerName = context.RouteData.Values["controller"].ToString();
            string actionName = context.RouteData.Values["action"].ToString();
            context.ExceptionHandled = true;

            context.Result = ReturnHandlerHelper.ProcessExceptionResponse(false, context.Exception.Message, context.Exception.StackTrace, null, context);
        }
    }
}
