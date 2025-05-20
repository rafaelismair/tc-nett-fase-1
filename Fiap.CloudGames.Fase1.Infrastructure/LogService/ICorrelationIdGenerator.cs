namespace Fiap.CloudGames.Fase1.Infrastructure.Logging
{
    public interface ICorrelationIdGenerator
    {
        string Get();
        void Set(string correlationId);
    }
}
