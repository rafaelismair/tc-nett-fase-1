using Fiap.CloudGames.Fase1.Application.DTOs.Promotions;
using Fiap.CloudGames.Fase1.Application.DTOs.Shared;
using Fiap.CloudGames.Fase1.Application.Services;
using Fiap.CloudGames.Fase1.Domain.Entities;
using Fiap.CloudGames.Fase1.Infrastructure.Data;
using Fiap.CloudGames.Fase1.Infrastructure.LogService.Services;
using Microsoft.EntityFrameworkCore;

namespace Fiap.CloudGames.Fase1.Tests;

public class PromotionServiceTests
{
    private readonly ApplicationDbContext _context;
    private readonly PromotionService _service;
    private readonly LogService<PromotionService> _logger;

    public PromotionServiceTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("PromotionTestDb")
            .Options;

        _logger = new LogService<PromotionService>();
        _context = new ApplicationDbContext(options);
        _service = new PromotionService(_context, _logger);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreatePromotion_WhenDataIsValid()
    {
        var dto = new CreatePromotionDto("Promo Title", "Description", 15, true);

        var result = await _service.CreateAsync(dto);

        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.Equal("Promo Title", result.Data.PromotionTitle);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnPaginatedPromotions()
    {
        _context.Promotions.Add(new Promotion("Title1", "Desc", 10, true));
        _context.Promotions.Add(new Promotion("Title2", "Desc", 20, true));
        await _context.SaveChangesAsync();

        var pagination = new PaginationDto() { PageNumber = 1, PageSize = 1 };

        var result = await _service.GetAllAsync(pagination);

        Assert.True(result.Success);
        Assert.Single(result.Data.Promotions);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoPromotionsExist()
    {
        var localContext = new ApplicationDbContext(
            new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("NewDbNoPromotion")
            .Options
            );

        var pagination = new PaginationDto();

        var result = await _service.GetAllAsync(pagination);

        Assert.True(result.Success);
        Assert.Empty(result.Data.Promotions);
        Assert.Equal(0, pagination.TotalPages);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnPromotion_WhenExists()
    {
        var promo = new Promotion("Get Me", "Desc", 5, true);
        _context.Promotions.Add(promo);
        await _context.SaveChangesAsync();

        var result = await _service.GetByIdAsync(promo.Id);

        Assert.True(result.Success);
        Assert.Equal(promo.Id, result.Data.Id);
    }

    [Fact]
    public async Task SetPromotionInactive_ShouldSetPromotionInactive_WhenActive()
    {
        var promo = new Promotion("Active Promo", "Desc", 10, true);
        _context.Promotions.Add(promo);
        await _context.SaveChangesAsync();

        var result = await _service.SetPromotionInactive(promo.Id);

        Assert.True(result.Success);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdatePromotion_WhenExists()
    {
        var promo = new Promotion("Old Title", "Old Desc", 10, true);
        _context.Promotions.Add(promo);
        await _context.SaveChangesAsync();

        var dto = new CreatePromotionDto("New Title", "New Desc", 20, true);

        var result = await _service.UpdateAsync(dto, promo.Id);

        Assert.True(result.Success);
        Assert.Equal("New Title", result.Data.PromotionTitle);
    }
}
