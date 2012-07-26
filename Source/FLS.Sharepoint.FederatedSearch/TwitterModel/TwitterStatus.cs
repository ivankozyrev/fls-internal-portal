namespace FederatedSearch.TwitterModel
{
    /// <summary>
    /// Represents a Twitter status message (a tweet).
    /// </summary>
    public class TwitterStatus
    {
        /// <summary>
        /// When a status message was created.
        /// </summary>
        public string CreatedAt { get; set; }

        /// <summary>
        /// The contents of a status message.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// The user associated with the status message.
        /// </summary>
        public TwitterUser User { get; set; }
    }
}
