using Device.Fingerprint.ExternalAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace Device.Fingerprint.ExternalAPI.Controllers
{
    [Route("api")]
    [ApiController]
    public class CustomerClassController : ControllerBase
    {
        private readonly ISubscriberService subscriberServices;
        private readonly IRiskLevelService riskLevelService;

        public CustomerClassController(ISubscriberService subscriberServices, IRiskLevelService riskLevelService)
        {
            this.subscriberServices = subscriberServices;
            this.riskLevelService = riskLevelService;
        }
    }
}
