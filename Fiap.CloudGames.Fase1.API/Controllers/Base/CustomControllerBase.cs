using Fiap.CloudGames.Fase1.Infrastructure.LogService.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Fiap.CloudGames.Fase1.API.Controllers.Base
{
    public abstract class CustomControllerBase<T> : ControllerBase
    {
        private readonly ILogService<T> _baseLogger;

        public CustomControllerBase(ILogService<T> baseLogger)
        {
            _baseLogger = baseLogger;
        }

        protected virtual IActionResult HandleResult<T>(T result, string notFoundMessage = "Resource not found.")
        {
            if (result == null)
                return NotFound(notFoundMessage);

            return Ok(result);
        }

        protected virtual IActionResult HandleBadRequest(string exceptionMessage, string errorMessage = "The request could no be processed.")
        {
            _baseLogger.LogError(exceptionMessage);

            return BadRequest(new { error = errorMessage });
        }

        protected virtual IActionResult HandleBadRequest(Exception ex, string errorMessage = "The request could no be processed.")
        {
            _baseLogger.LogError(ex);

            return BadRequest(new { error = errorMessage });
        }

        protected virtual IActionResult HandleException(Exception ex, string message = "An unexpected error occurred.")
        {
            _baseLogger.LogError(ex);

            return StatusCode(StatusCodes.Status500InternalServerError, new { error = message });
        }
    }
}
