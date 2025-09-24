using Device.Fingerprint.ExternalAPI.Models;

namespace Device.Fingerprint.ExternalAPI.Middlewares
{
    public class JwtValidationMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IAppSettings appSettings;
        private readonly IConfiguration configuration;

        public JwtValidationMiddleware(RequestDelegate next, IAppSettings appSettings, IConfiguration configuration)
        {
            this.next = next;
            this.appSettings = appSettings;
            this.configuration = configuration;
        }

        public async Task Invoke(HttpContext context)
        {
            if ((this.appSettings.Environment == "PROD" || this.appSettings.Environment == "UAT") &&
                !context.Request.Path.Value.Contains("generatetoken/audience") && !context.Request.Path.Value.Contains("whoami"))
            {
                var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(' ').LastOrDefault();

                if (string.IsNullOrWhiteSpace(token) || !ValidateToken(context, token))
                {
                    context.Response.Clear();
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;

                    return;
                }
            }

            await next(context);
        }

        private bool ValidateToken(HttpContext context, string token)
        {
            if (!string.IsNullOrWhiteSpace(token))
            {
                var jwtSetting = configuration.GetSection("AppConfig:JWT:SubscriberConfig").Get<IEnumerable<SubscriberTokenModel>>();
                var subscriber = context.Request.Path.Value.Split('/')[3];

                var matchingToken = jwtSetting
                    .FirstOrDefault(item => item.Token.Equals(token, StringComparison.OrdinalIgnoreCase));

                if (matchingToken != null)
                {
                    var subscribers = matchingToken.Subscribers.Split(',');
                    return subscribers.Length > 0 && Array.Exists(subscribers, x => x.Trim().Equals(subscriber, StringComparison.OrdinalIgnoreCase));
                }
            }

            return false;

        }
    }
}
