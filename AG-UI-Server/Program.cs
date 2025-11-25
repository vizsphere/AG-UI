using AG_UI_Server.Model;
using Azure.AI.OpenAI;
using Azure.Identity;
using Microsoft.Agents.AI;
using Microsoft.Agents.AI.Hosting.AGUI.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using OpenAI.Chat;
using System.ComponentModel;

(string model, string endpoint) = (Environment.GetEnvironmentVariable("AZURE_OPEN_AI_MODEL", EnvironmentVariableTarget.User) ?? "",
                                   Environment.GetEnvironmentVariable("AZURE_OPEN_AI_ENDPOINT", EnvironmentVariableTarget.User) ?? "");

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient().AddLogging();
builder.Services.AddAGUI();

WebApplication app = builder.Build();

ChatClient chatClient = new AzureOpenAIClient(
        new Uri(endpoint),
        new AzureCliCredential())
    .GetChatClient(model);

AIAgent agent = chatClient.AsIChatClient().CreateAIAgent(
    name: "AGUIAssistant",
    instructions: "You are a helpful assistant.",
    tools: new[] { AIFunctionFactory.Create(GetRandomSpeaker) });

// Map the AG-UI agent endpoint
app.MapAGUI("/", agent);

await app.RunAsync();

[Description("Get random speaker information.")]
static Speaker GetRandomSpeaker()
{
    var speakers = new List<Speaker>()
    {
        new Speaker() { Id = 1, Name = "Dave Ramsey", Bio = "Financial author and motivational speaker.", WebSite = "http://www.daveramsey.com" },
        new Speaker() { Id = 2, Name = "Tony Robbins", Bio = "Motivational speaker and self-help author.", WebSite = "http://www.tonyrobbins.com" },
        new Speaker() { Id = 3, Name = "Nick Vujicic", Bio = "Christian evangelist and motivational speaker.", WebSite = "http://www.nickvujicic.com" },
        new Speaker() { Id = 4, Name = "Eckhart Tolle", Bio = "Author of The Power of Now.", WebSite = "http://www.eckharttolle.com" },
        new Speaker() { Id = 5, Name = "Louise Hay", Bio = "Motivational author and founder of Hay House.", WebSite = "http://www.louisehay.com" },
        new Speaker() { Id = 6, Name = "Chris Gardner", Bio = "Entrepreneur and motivational speaker.", WebSite = "http://www.chrisgardnermedia.com" },
        new Speaker() { Id = 7, Name = "Robert Kiyosaki", Bio = "Businessman and financial literacy activist.", WebSite = "http://www.richdad.com" }
    };
    Random rand = new();
    int index = rand.Next(speakers.Count);
    return speakers[index];
}