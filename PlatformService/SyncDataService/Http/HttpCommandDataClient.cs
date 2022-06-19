using System.Text;
using System.Text.Json;
using PlatformService.Models.DTOs;

namespace PlatformService.SyncDataService.Http{
    public class HttpCommandDataClient : ICommandDataClient
    {
        private readonly HttpClient _http;
        private readonly ILogger<HttpCommandDataClient> _logger;
        private readonly IConfiguration _configuration;

        public HttpCommandDataClient(HttpClient http, ILogger<HttpCommandDataClient> logger, IConfiguration configuration)
        {
            _http = http;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task SendPlatformToCommand(PlatformResponseDTO response)
        {
            try
            {
                var httpContent = new StringContent(
                    JsonSerializer.Serialize(response),
                    Encoding.UTF8,
                    "application/json"
                );

                var result = await _http.PostAsync(_configuration["CommandService"], httpContent).ConfigureAwait(false);
                if(result.IsSuccessStatusCode)
                    _logger.LogInformation($"--> POST Request successful: { result.Content }");
                else
                    _logger.LogError($"--> POST Request Error: { result.Content }");
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(SendPlatformToCommand));
                throw;
            }
        }
    }
}