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

        protected virtual IActionResult HandleResult<T>(T result, string notFoundMessage = "Recurso não encontrado.")
        {
            if (result == null)
                return NotFound(notFoundMessage);

            return Ok(result);
        }

        protected virtual IActionResult HandleBadRequest(string exceptionMessage, string errorMessage = "A requisição não pôde ser executada.")
        {
            _baseLogger.LogError(exceptionMessage);

            return BadRequest(new { error = errorMessage });
        }

        protected virtual IActionResult HandleBadRequest(Exception ex, string errorMessage = "A requisição não pôde ser processada.")
        {
            _baseLogger.LogError(ex);

            return BadRequest(new { error = errorMessage });
        }

        protected virtual IActionResult HandleException(Exception ex, string message = "Ocorreu um erro inesperado.")
        {
            _baseLogger.LogError(ex);

            return StatusCode(StatusCodes.Status500InternalServerError, new { error = message });
        }

        protected virtual IActionResult HandleException(string exceptionMessage, string message = "Ocorreu um erro inesperado.")
        {
            _baseLogger.LogError(exceptionMessage);

            return StatusCode(StatusCodes.Status500InternalServerError, new { error = message });
        }
    }
}
