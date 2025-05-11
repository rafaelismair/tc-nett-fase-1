using Fiap.CloudGames.Fase1.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Fiap.CloudGames.Fase1.Infrastructure.Helpers
{
    public static class ReturnHandlerHelper
    {
        public static ActionResult ProcessResult(bool success, string message, string localError, object model = null)
        {
            return ProcessResult(success, message, localError, model, 200);
        }
        public static ActionResult ProcessResult(bool success, string message, string localError, object model, int statusCode)
        {
            var returnModel = new ReturnModel() { Result = new ResultModel[1] };

            if (success)
            {
                returnModel.Status = 1;
                returnModel.Object = model;
                returnModel.Result[0] = new ResultModel() { Status = 1 };
            }
            else
            {
                returnModel.Status = 0;
                returnModel.Result[0] = new ResultModel()
                {
                    Status = 0,
                    Message = message,
                    LocalError = localError
                };
            }
            return new ObjectResult(returnModel) { StatusCode = statusCode };
        }

        public static ActionResult ProcessExceptionResponse(bool success, string message, string localError, object model, ExceptionContext contextEx)
        {
            var exceptionString = message ?? string.Empty;
            string stackTraceString;
            string bodyString;
            string queryString;

            if (!(contextEx is null))
            {
                stackTraceString = contextEx?.Exception?.StackTrace ?? string.Empty;
                var bodyStream = contextEx?.HttpContext?.Request?.Body;
                queryString = contextEx?.HttpContext?.Request?.QueryString.Value ?? string.Empty;
                exceptionString = message ?? string.Empty;

                if (bodyStream != null && bodyStream.CanSeek)
                {
                    using (var reader = new StreamReader(bodyStream))
                    {
                        bodyStream.Seek(0, SeekOrigin.Begin);
                        bodyString = reader.ReadToEnd();
                    }
                }
            }

            // TODO: Log the unhandled error
                //Log.Error($@"
                //    An unhandled exception occurred during the request.
                //    QueryString: {queryString}
                //    Exception: {exceptionString}
                //    StackTrace: {stackTraceString}
                //");

            return ProcessResult(success, message ?? string.Empty, localError, model, 500);
        }
    }
}
