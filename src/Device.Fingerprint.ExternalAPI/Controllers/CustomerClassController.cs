using Device.Fingerprint.ExternalAPI.Models;
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

        [HttpGet("GetRiskLevelBySubscriber/{subscriberName}/{lastId:long}")]
        public ActionResult<CustomerClassRiskLevelModel> GetRiskLevelBySubscriber(string subscriberName, long lastId)
        {
            try
            {
                var subscriber = subscriberServices.GetSubscriberByName(subscriberName);

                if (subscriber != null && subscriber.SubscriberId > 0)
                {
                    var trans = riskLevelService.GetCustomerClassRiskLevel(subscriber.SubscriberId, lastId, 1000);

                    var transformedTrans = trans.Select(x => new CustomerClassRiskLevelModel
                    {
                        Account = x.RegisterName,
                        Id = x.Id,
                        RiskLevel = x.RiskLevel,
                    });

                    return Ok(new CustomerClassRiskLevelList
                    {
                        Trans = transformedTrans,
                        StatusCode = System.Net.HttpStatusCode.OK,
                        ErrorMessage = string.Empty
                    });
                }
                else
                {
                    return Ok(new CustomerClassRiskLevelList
                    {
                        Trans = null,
                        StatusCode = System.Net.HttpStatusCode.NotFound,
                        ErrorMessage = "Subscriber cannot be null"
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new CustomerClassRiskLevelList
                {
                    Trans = null,
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    ErrorMessage = ex.Message
                });
            }
        }
    }
}
