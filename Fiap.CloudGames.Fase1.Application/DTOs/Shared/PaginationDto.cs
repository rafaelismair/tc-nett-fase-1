namespace Fiap.CloudGames.Fase1.Application.DTOs.Shared;

public record PaginationDto
{
    public int PageSize { get; set; } = 10;
    public int PageNumber { get; set; } = 1;
    public int TotalPages { get; private set; }
    public void SetTotalPages(int totalItems)
    {
        TotalPages = (int)Math.Ceiling((double)totalItems / PageSize);
    }
}
