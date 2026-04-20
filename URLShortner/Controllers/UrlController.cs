using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("api/[controller]")]
public class UrlController : ControllerBase
{
    private readonly iUrlShorteningService _service;
    public UrlController(iUrlShorteningService service)
    {
        _service = service;
    }

    [HttpPost("CreateShortUrl")]
    public async Task<IActionResult> CreateShortUrl([FromBody] ShortenUrlRequest dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            var shortUrl = await _service.ShortenUrlRequest(dto, HttpContext);
            return Ok(new { shortUrl });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("/{code}")]
    public async Task<IActionResult> ReturnUrl(string code)
    {
        var result = await _service.URlReturn(code);
        if (string.IsNullOrWhiteSpace(result))
            return NotFound("No url like that");
        return Redirect(result);
    }
}