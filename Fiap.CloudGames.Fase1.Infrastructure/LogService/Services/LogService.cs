using Fiap.CloudGames.Fase1.Infrastructure.Logging;
using Fiap.CloudGames.Fase1.Infrastructure.LogService.Interfaces;
using Microsoft.Extensions.Logging;

namespace Fiap.CloudGames.Fase1.Infrastructure.LogService.Services
{
    public class LogService<T> : ILogService<T>
    {
        protected readonly ILogger<T> _logger;
        protected readonly ICorrelationIdGenerator _correlationId;

        public LogService() { }

        public LogService(ILogger<T> logger, ICorrelationIdGenerator correlationId)
        {
            _logger = logger;
            _correlationId = correlationId;
        }

        public virtual void LogInformation(string message)
        {
            _logger.LogInformation(message);
        }

        public virtual void LogWarning(string message)
        {
            _logger.LogWarning(message);
        }

        public virtual void LogError(string message)
        {
            _logger.LogError(message);
        }
        
        public virtual void LogError(Exception ex)
        {
            _logger.LogError($"{ex.Message} {Environment.NewLine} | {ex.StackTrace} |");
        }


    }
}
