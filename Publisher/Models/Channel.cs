namespace Publisher.Models
{
    public class Channel
    {
        public string channelName { get; set; }

        List<Message> messages { get; set; }

        List<Subscriber> subscribers { get; set; }
    }
}
