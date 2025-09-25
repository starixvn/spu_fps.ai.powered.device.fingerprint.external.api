namespace Device.Fingerprint.ExternalAPI.Models;

public class WhoamiModel
{
    public string Environment { get; set; } = string.Empty;

    public string ServerName { get; set; } = string.Empty;

    public string LocalIP { get; set; } = string.Empty;

    public DateTime ServerTime { get; set; }

    public string RealIP { get; set; } = string.Empty;

    public string FakeIP { get; set; } = string.Empty;

    public string XFW { get; set; } = string.Empty;

    public string ClientIP { get; set; } = string.Empty;

    public string ActualIP { get; set; } = string.Empty;

}
