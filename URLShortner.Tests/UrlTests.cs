using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

using URL.Services;

public class UrlControllerTests
{
    private readonly Mock<iUrlShorteningService> _serviceMock;
    private readonly UrlController _controller;

    public UrlControllerTests()
    {
        _serviceMock = new Mock<iUrlShorteningService>();
        _controller = new UrlController(_serviceMock.Object);
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };
    }

    [Fact]
    public async Task CreateShortUrl_ValidRequest_ReturnsOk()
    {
        var dto = new ShortenUrlRequest { Url = "https://google.com" };
        _serviceMock.Setup(s => s.ShortenUrlRequest(dto, It.IsAny<HttpContext>()))
            .ReturnsAsync("http://localhost:5272/abc1234");

        var result = await _controller.CreateShortUrl(dto);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(ok.Value);
    }

    [Fact]
    public async Task CreateShortUrl_InvalidUrl_ReturnsBadRequest()
    {
        var dto = new ShortenUrlRequest { Url = "not-a-url" };
        _serviceMock.Setup(s => s.ShortenUrlRequest(dto, It.IsAny<HttpContext>()))
            .ThrowsAsync(new ArgumentException("Invalid URL."));

        var result = await _controller.CreateShortUrl(dto);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task ReturnUrl_ValidCode_ReturnsRedirect()
    {
        _serviceMock.Setup(s => s.URlReturn("abc1234"))
            .ReturnsAsync("https://google.com");

        var result = await _controller.ReturnUrl("abc1234");

        var redirect = Assert.IsType<RedirectResult>(result);
        Assert.Equal("https://google.com", redirect.Url);
    }

    [Fact]
    public async Task ReturnUrl_InvalidCode_ReturnsNotFound()
    {
        _serviceMock.Setup(s => s.URlReturn("invalid"))
            .ReturnsAsync(string.Empty);

        var result = await _controller.ReturnUrl("invalid");

        Assert.IsType<NotFoundObjectResult>(result);
    }
}

public class UrlShorteningServiceTests
{
    private readonly ApplicationDbContext _dbContext;
    private readonly UrlShorteningService _service;

    public UrlShorteningServiceTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _dbContext = new ApplicationDbContext(options);
        _service = new UrlShorteningService(_dbContext);
    }

    [Fact]
    public async Task GenerateUniqueCode_ReturnsCorrectLength()
    {
        var code = await _service.GenerateUniqueCode();

        Assert.Equal(UrlShorteningService.NumberOfCharsInShortLink, code.Length);
    }

    [Fact]
    public async Task GenerateUniqueCode_ReturnsUniqueCode()
    {
        var code1 = await _service.GenerateUniqueCode();
        var code2 = await _service.GenerateUniqueCode();

        // Not guaranteed but statistically safe
        Assert.NotEqual(code1, code2);
    }

    [Fact]
    public async Task URlReturn_ValidCode_ReturnsLongUrl()
    {
        _dbContext.ShortenUrls.Add(new ShortenUrl
        {
            Id = Guid.NewGuid(),
            Code = "abc1234",
            LongUrl = "https://google.com",
            ShortUrl = "http://localhost/abc1234",
            CreatedOnUtc = DateTime.UtcNow
        });
        await _dbContext.SaveChangesAsync();

        var result = await _service.URlReturn("abc1234");

        Assert.Equal("https://google.com", result);
    }

    [Fact]
    public async Task ShortenUrlRequest_ValidUrl_ReturnsShortUrl()
    {
        var dto = new ShortenUrlRequest { Url = "https://google.com" };
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Scheme = "http";
        httpContext.Request.Host = new HostString("localhost:5272");

        var result = await _service.ShortenUrlRequest(dto, httpContext);

        Assert.StartsWith("http://localhost:5272/", result);
        Assert.Equal(7, result.Split('/').Last().Length);
    }

    [Fact]
    public async Task ShortenUrlRequest_InvalidUrl_ThrowsArgumentException()
    {
        var dto = new ShortenUrlRequest { Url = "not-a-url" };
        var httpContext = new DefaultHttpContext();

        await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.ShortenUrlRequest(dto, httpContext));
    }
}