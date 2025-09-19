using Device.Fingerprint.ExternalAPI.Controllers;
using Device.Fingerprint.ExternalAPI.Models;
using Device.FingerPrint.ExternalAPI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Device.Fingerprint.ExternalAPI.Tests.Controllers;

public class CommonControllerTests
{
    private readonly Mock<HttpContext> _mockHttpContext;
    private readonly CommonController _controller;

    public CommonControllerTests()
    {
        var mockHttpConnectionFeature = new Mock<IHttpConnectionFeature>();
        mockHttpConnectionFeature.Setup(a => a.LocalIpAddress).Returns(System.Net.IPAddress.Loopback);

        var mockAppSettings = new Mock<IAppSettings>();
        mockAppSettings.Setup(a => a.Environment).Returns("TestEnvironment");

        _controller = new CommonController(mockAppSettings.Object);
        _controller.ControllerContext = new ControllerContext();
        _controller.ControllerContext.HttpContext = new DefaultHttpContext();
        _controller.ControllerContext.HttpContext.Request.Headers["X-Remote-IP"] = System.Net.IPAddress.Loopback.ToString();
        _controller.ControllerContext.HttpContext.Request.Headers["X-Actual-IP"] = System.Net.IPAddress.Loopback.ToString();
        _controller.ControllerContext.HttpContext.Features.Set(mockHttpConnectionFeature.Object);
    }

    [Fact]
    public void Index_ShouldReturnWhoamiModel()
    {
        // Arrange

        // Act
        var result = _controller.Index();

        // Assert
        Assert.NotNull(result);
        Assert.IsType<WhoamiModel>(result);
        Assert.Equal("TestEnvironment", result.Environment);
        Assert.Equal(Environment.MachineName, result.ServerName);
        Assert.True((DateTime.Now - result.ServerTime).TotalSeconds < 5); // ServerTime should be recent
        Assert.NotNull(result.LocalIP);
        Assert.NotNull(result.ClientIP);
        Assert.NotNull(result.FakeIP);
        Assert.NotNull(result.RealIP);
        Assert.NotNull(result.XFW);
        Assert.NotNull(result.ActualIP);
    }
}
