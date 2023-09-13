using Publisher.Models;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseHttpsRedirection();

List<Channel> channels = new List<Channel>();

app.MapPost("api/channels/{channelName}/subscribe", async (string channelName, Subscriber sub) => 
{
    Channel channel = channels.FirstOrDefault(c => c.channelName == channelName);

    if (channel == null)
    {
        channel = new Channel 
        { 
            channelName = channelName,
            messages = new List<Message>(),
            subscribers = new List<Subscriber>()
        };
        
        channels.Add(channel);
    }

    channel.subscribers.Add(sub);
});

app.MapPost("api/channels/{channelName}/publish", async (string channelName, Message msg) =>
{
    throw new NotImplementedException();
});

app.MapGet("api/subscribers/{subscriberName}/messages", async (string subscriberName) =>
{
    return new List<string>();
    //throw new NotImplementedException();
});

app.Run();
