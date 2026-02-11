using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using OpenAI;
using System.ClientModel;

// get Credentials from user secrets

IConfigurationRoot config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();

var credential = new ApiKeyCredential(config["GitHubModels:Token"] ?? throw new InvalidCastException("Invalid credentials"));

var option = new OpenAIClientOptions()
{
    Endpoint = new Uri("https://models.github.ai/inference")
};

// create a chat client

IChatClient client = new OpenAIClient(credential, option).GetChatClient("openai/gpt-4o-mini").AsIChatClient();



//while (true)
//{
//    //Console.WriteLine("Enter your question OR Press ctrl+c for exit:");
//    //if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.C && Console.ReadKey(true).Key == ConsoleKey.CrSel)
//    //{
//    //    break;
//    //}
//    ////send prompt and get response
//    //var prompt = Console.ReadLine() ?? string.Empty;

//    #region Basic Completion
//    // var response = await client.GetResponseAsync(prompt);
//    // Console.WriteLine($"Assistant >>> {response}");
//    //Console.WriteLine($"Token used: in = {response.Usage?.InputTokenCount}, out:{response.Usage?.OutputTokenCount} ");

//    #endregion

//    #region Streaming
//    //var streamingResponse = client.GetStreamingResponseAsync(prompt);
//    //await foreach (var message in streamingResponse)
//    //{
//    //    Console.Write(message);

//    //}
//    #endregion

//}

#region Classification
//var classificationPrompt = """
//    Please classify the following sentences into categories: 
//    - 'complaint' 
//    - 'suggestion' 
//    - 'praise' 
//    - 'other'.

//    1) "I love the new layout!"
//    2) "You should add a night mode."
//    3) "When I try to log in, it keeps failing."
//    4) "This app is decent."
//    """;

//Console.WriteLine($"user >>> {classificationPrompt}");

//ChatResponse classificationResponse = await client.GetResponseAsync(classificationPrompt);

//Console.WriteLine($"assistant >>>\n{classificationResponse}");

#endregion

#region Summarization
//var summaryPrompt = """
//Summarize the following blog in 1 concise sentences:

//"Microservices architecture is increasingly popular for building complex applications, but it comes with additional overhead. It's crucial to ensure each service is as small and focused as possible, and that the team invests in robust CI/CD pipelines to manage deployments and updates. Proper monitoring is also essential to maintain reliability as the system grows."
//""";

//Console.WriteLine($"user >>> {summaryPrompt}");

//ChatResponse summaryResponse = await client.GetResponseAsync(summaryPrompt);

//Console.WriteLine($"assistant >>> \n{summaryResponse}");

#endregion

#region Sentiment Analysis
var analysisPrompt = """
        You will analyze the sentiment of the following product reviews. 
        Each line is its own review. Output the sentiment of each review in a bulleted list and then provide a generate sentiment of all reviews.

        I bought this product and it's amazing. I love it!
        This product is terrible. I hate it.
        I'm not sure about this product. It's okay.
        I found this product based on the other reviews. It worked for a bit, and then it didn't.
        """;

Console.WriteLine($"user >>> {analysisPrompt}");

ChatResponse responseAnalysis = await client.GetResponseAsync(analysisPrompt);

Console.WriteLine($"assistant >>> \n{responseAnalysis}");


#endregion





