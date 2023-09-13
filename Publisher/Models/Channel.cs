namespace Publisher.Models
{
    public class Channel
    {
        public string channelName { get; set; }

        public List<Message> messages { get; set; }

        public List<Subscriber> subscribers { get; set; }
    }
}
