using Microsoft.AspNetCore.Mvc;

namespace FetchBECData.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BECController : ControllerBase
    {
        private readonly ILogger<BECController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public BECController(ILogger<BECController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string url)
        {
            var client = _httpClientFactory.CreateClient();

            HttpResponseMessage response;
            try
            {
                response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                return BadRequest($"Error fetching the file: {ex.Message}");
            }

            var content = await response.Content.ReadAsByteArrayAsync();
            var contentType = "text/csv";
            var fileName = url.Substring(url.LastIndexOf('/') + 1);

            return File(content, contentType, fileName);
        }
    }
}
