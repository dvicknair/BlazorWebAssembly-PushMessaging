namespace PushAPI.Shared
{
    public class PushMessageSubscription
    {
        public int? PushMessageSubscriptionId { get; set; }

        public string? UserId { get; set; }

        public string? Url { get; set; }

        public string? P256dh { get; set; }

        public string? Auth { get; set; }
    }
}
