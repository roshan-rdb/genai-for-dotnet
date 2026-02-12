// get Credentials from user secrets

using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using OpenAI;
using System.ClientModel;
using System.Numerics.Tensors;

IConfigurationRoot config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();

var credential = new ApiKeyCredential(config["GitHubModels:Token"] ?? throw new InvalidCastException("Invalid credentials"));

var option = new OpenAIClientOptions()
{
    Endpoint = new Uri("https://models.github.ai/inference")
};

// create a chat client

IChatClient client = new OpenAIClient(credential, option).GetChatClient("openai/gpt-4o-mini").AsIChatClient();

IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator = 
    new OpenAIClient(credential, option).GetEmbeddingClient("openai/text-embedding-3-small").AsIEmbeddingGenerator();

//var embedding = await embeddingGenerator.GenerateVectorAsync("Hello world");
//Console.WriteLine($"Embedding dimension: {embedding.Span.Length}");
///foreach (var value in embedding.Span)
//{
  //  Console.Write("{0:0.00}, " , value);
//}

var catvector = await embeddingGenerator.GenerateVectorAsync("cat");
var dogvector = await embeddingGenerator.GenerateVectorAsync("dog");
var kittenvector = await embeddingGenerator.GenerateVectorAsync("kitten");

Console.WriteLine($"cat-dog similarity: {TensorPrimitives.CosineSimilarity(catvector.Span, dogvector.Span):F2}");
Console.WriteLine($"cat-kitten similarity: {TensorPrimitives.CosineSimilarity(catvector.Span, kittenvector.Span):F2}");
Console.WriteLine($"dog-kitten similarity: {TensorPrimitives.CosineSimilarity(dogvector.Span, kittenvector.Span):F2}");