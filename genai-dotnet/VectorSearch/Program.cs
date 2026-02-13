using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel.Connectors.InMemory;
using OpenAI;
using System.ClientModel;
using VectorSearch;

IConfigurationRoot config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();

var credential = new ApiKeyCredential(config["GitHubModels:Token"] ?? throw new InvalidCastException("Invalid credentials"));

var option = new OpenAIClientOptions()
{
    Endpoint = new Uri("https://models.github.ai/inference")
};

IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator =
    new OpenAIClient(credential, option)
    .GetEmbeddingClient("openai/text-embedding-3-small")
    .AsIEmbeddingGenerator();

var vectorStore = new InMemoryVectorStore();
var moviesStore = vectorStore.GetCollection<int, Movie>("movies");
await moviesStore.EnsureCollectionExistsAsync();

foreach (var movie in MovieData.Movies)
{
    movie.Vector = await embeddingGenerator.GenerateVectorAsync(movie.Description);
    await moviesStore.UpsertAsync(movie);
}

//var query = "I want to see familly friendly movies";
var query = "I want to see science fiction movies";
var queryEmbedding = await embeddingGenerator.GenerateVectorAsync(query);

var searchResults = moviesStore.SearchAsync(queryEmbedding, top: 2);

await foreach( var result in searchResults)
{
    Console.WriteLine($"Title: {result.Record.Title}");
    Console.WriteLine($"Description : {result.Record.Description}");
    Console.WriteLine($"Similarity Score: {result.Score}");
    Console.WriteLine();
}