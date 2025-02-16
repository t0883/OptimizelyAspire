using OptimizelyAspire.AppHost;

var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

var ollama = builder.AddOllama("ollama")
    .WithGPUSupport()
    .WithDataVolume()
    .WithOpenWebUI();

var chat = ollama.AddModel("chat", "llama3.2");
var embeddings = ollama.AddModel("embedding", "all-minilm");

var apiService = builder.AddProject<Projects.OptimizelyAspire_ApiService>("apiservice")
    .WithReference(chat)
    .WithReference(embeddings)
    .WaitFor(chat)
    .WaitFor(embeddings)
    .WithScalar();

builder.AddProject<Projects.OptimizelyAspire_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WaitFor(cache)
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
