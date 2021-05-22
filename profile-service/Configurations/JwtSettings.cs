using profile_service.Interfaces;

namespace profile_service.Configurations
{
    public class JwtSettings : IJwtSettings
    {
        public string Secret { get; set; }
    }
} 