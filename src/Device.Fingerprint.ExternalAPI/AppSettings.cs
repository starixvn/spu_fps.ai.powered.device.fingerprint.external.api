namespace Device.FingerPrint.ExternalAPI
{
    using System.ComponentModel;

    public interface IAppSettings
    {
        string SentryDsn { get; }

        string Environment { get; }
    }

    public class AppSettings : IAppSettings
    {
        private readonly IConfiguration configuration;

        private readonly string sentryDsn;

        private readonly string environment;

        public AppSettings(IConfiguration configuration)
        {
            this.configuration = configuration;

            sentryDsn = GetValue<string>("Sentry:Dsn");

            environment = GetValue<string>("Environment");
        }

        public string SentryDsn => sentryDsn;

        public string Environment => environment;

        public T GetValue<T>(string key)
        {
            var value = configuration[key];

            if (string.IsNullOrEmpty(value))
            {
                throw new InvalidOperationException($"Configuration value for key '{key}' is null or empty.");
            }

            var typeConverter = TypeDescriptor.GetConverter(typeof(T));

            if (typeConverter == null)
            {
                throw new InvalidOperationException($"No type converter available for type '{typeof(T)}'.");
            }

            var convertedValue = typeConverter.ConvertFromString(value);

            if (convertedValue == null)
            {
                throw new InvalidCastException($"Failed to convert configuration value for key '{key}' to type '{typeof(T)}'.");
            }

            return (T)convertedValue;
        }
    }
}
