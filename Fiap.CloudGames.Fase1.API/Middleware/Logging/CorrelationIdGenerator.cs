using Fiap.CloudGames.Fase1.Infrastructure.Logging;

namespace Fiap.CloudGames.Fase1.API.Middleware.Logging
{
    public class CorrelationIdGenerator : ICorrelationIdGenerator
    {
        private static string _correlationId;

        public string Get() => _correlationId;

        public void Set(string correlationId) => _correlationId = correlationId;
    }
}
