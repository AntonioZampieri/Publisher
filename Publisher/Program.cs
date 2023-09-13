using Microsoft.AspNetCore.SignalR;
using Publisher.Models;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseHttpsRedirection();

/// list of existing channels
List<Channel> channels = new List<Channel>();

/// <summary>
/// POST method to subscribe to a selected channel
/// </summary>
/// <param name="channelName"> name of the channel to subscribe to </param>
/// <param name="sub"> subscriber to add to the channel </param>
/// <returns> OK response </returns>
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

    if(channel.subscribers.FirstOrDefault(s => s.SubscriberName == sub.SubscriberName) == null)
        channel.subscribers.Add(sub);

    return Results.Ok();
});

/// <summary>
/// POST method to publish a message on a selected channel
/// </summary>
/// <param name="channelName"> name of the channel to publish the message on </param>
/// <param name="message"> message to publish on the selected channel </param>
/// <returns> OK response </returns>
app.MapPost("api/channels/{channelName}/publish", async (string channelName, Message message) =>
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

    channel.messages.Add(message);

    return Results.Ok();
});

/// <summary>
/// GET method to retrieve all the messages for a selected subscriber
/// </summary>
/// <param name="subscriberName"> name of the subscriber to retrieve messages for </param>
/// <returns> list of message texts </returns>
app.MapGet("api/subscribers/{subscriberName}/messages", async (string subscriberName) =>
{
    IEnumerable<Channel> subscribedChannels = channels.Where((channel) =>
    {
        return channel.subscribers.FirstOrDefault(s => s.SubscriberName == subscriberName) != null;
    });

    List<string> messagesToSend = new List<string>();
    subscribedChannels.AsParallel().ForAll(C =>
    {
        C.messages.AsParallel().ForAll(message => { messagesToSend.Add(message.MessageText); });
    });

    return Results.Ok(messagesToSend);
});

app.Run();
