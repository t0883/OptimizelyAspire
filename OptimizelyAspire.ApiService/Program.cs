using Microsoft.Extensions.AI;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

builder.AddOllamaSharpChatClient("chat");
builder.AddOllamaSharpEmbeddingGenerator("embeddings");

// Add services to the container.
builder.Services.AddProblemDetails();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapGet("chat/{chat}", async (IChatClient chatClient, string chat) =>
{
    var response = await chatClient.CompleteAsync(chat);

    return response;
});

app.MapDefaultEndpoints();

app.Run();
