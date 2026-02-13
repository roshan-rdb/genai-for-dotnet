using Microsoft.Extensions.AI;
using Microsoft.SemanticKernel.Connectors.InMemory;
using OllamaSharp;
using VectorSearch_Ollama;


IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator =
    new OllamaApiClient(new Uri("http://localhost:11434"),"all-minilm");

var vectorStore = new InMemoryVectorStore();
var moviesStore = vectorStore.GetCollection<int, Movie>("movies");
await moviesStore.EnsureCollectionExistsAsync();

foreach (var movie in MovieData.Movies)
{
    movie.Vector = await embeddingGenerator.GenerateVectorAsync(movie.Description);
    await moviesStore.UpsertAsync(movie);
}

var query = "I want to see science fiction movies";
var queryEmbedding = await embeddingGenerator.GenerateVectorAsync(query);

var searchResults = moviesStore.SearchAsync(queryEmbedding, top: 2);

await foreach (var result in searchResults)
{
    Console.WriteLine($"Title: {result.Record.Title}");
    Console.WriteLine($"Description : {result.Record.Description}");
    Console.WriteLine($"Similarity Score: {result.Score}");
    Console.WriteLine();
}