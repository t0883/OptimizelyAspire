using OptimizelyAspire.AppHost;

var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

var ollama = builder.AddOllama("ollama")
    .WithGPUSupport()
    .WithDataVolume()
    .WithOpenWebUI();

var chat = ollama.AddModel("chat", "llama3.2");
var embeddings = ollama.AddModel("embedding", "all-minilm");

var optimizelyEcommerce = builder.AddProject<Projects.OptimizelyCommerce>("optimizely");

var apiService = builder.AddProject<Projects.OptimizelyAspire_ApiService>("apiservice")
    .WithReference(chat)
    .WithReference(embeddings)
    .WaitFor(chat)
    .WaitFor(embeddings)
    .WaitFor(optimizelyEcommerce)
    .WithReference(optimizelyEcommerce)
    .WithScalar();

builder.AddProject<Projects.OptimizelyAspire_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WaitFor(cache)
    .WithReference(apiService)
    .WaitFor(apiService)
    .WaitFor(optimizelyEcommerce)
    .WithReference(optimizelyEcommerce);



builder.Build().Run();
