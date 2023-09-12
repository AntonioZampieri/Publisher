//using System.Web.Http;

using Publisher.Models;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseHttpsRedirection();

app.MapPost("api/channels/{channelName}/subscribe", async (string channelName, Subscriber sub) => 
{
    throw new NotImplementedException();
});

app.MapPost("api/channels/{channelName}/publish", async (string channelName, Message msg) =>
{
    throw new NotImplementedException();
});

app.Run();
