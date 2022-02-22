using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DoctorSystem.Services
{
    public class RouterService
    {
        private readonly ILogger<EmailService> _logger;
        private readonly IConfiguration _config;

        public RouterService(ILogger<EmailService> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        public string Route(string Url)
        {
            return _config["Root"] + Url;
        }
    }
}
