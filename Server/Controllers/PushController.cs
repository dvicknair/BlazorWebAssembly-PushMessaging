using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PushAPI.Shared;
using System.Text.Json;
using WebPush;

namespace PushAPI.Server.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class PushController : ControllerBase
    {
        private static List<PushSubscription> PushSubsciptions = new();
        private readonly ILogger<PushController> _logger;

        public PushController(ILogger<PushController> logger) => _logger = logger;

        [HttpPut]
        public IActionResult Subscribe(PushMessageSubscription subscription)
        {
            PushSubscription(subscription);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Send()
        {
            await SendPush();
            return Ok();
        }

        private void PushSubscription(PushMessageSubscription subscription) => PushSubsciptions.Add(new PushSubscription(subscription.Url, subscription.P256dh, subscription.Auth));

        private async Task SendPush()
        {
            // For a real application, generate your own
            var publicKey = "BLC8GOevpcpjQiLkO7JmVClQjycvTCYWm6Cq_a7wJZlstGTVZvwGFFHMYfXt6Njyvgx_GlXJeo5cSiZ1y4JOx1o";
            var privateKey = "OrubzSz3yWACscZXjFQrrtDwCKg-TGFuWhluQ2wLXDo";
            var vapidDetails = new VapidDetails("mailto:mail@example.com", publicKey, privateKey);
            var webPushClient = new WebPushClient();
            var payload = JsonSerializer.Serialize(new { message = "Test message", url = "testUrl", });

            foreach (var subscription in PushSubsciptions)
            {
                try
                {
                    await webPushClient.SendNotificationAsync(subscription, payload, vapidDetails);
                }
                catch (Exception e)
                {
                    _logger.LogError("Error sending push notification: " + e.Message);
                }
            }
        }
    }
}
