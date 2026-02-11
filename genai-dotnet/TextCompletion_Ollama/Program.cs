using Microsoft.Extensions.AI;
using OllamaSharp;

IChatClient client = new OllamaApiClient("http://localhost:11434","llama3.2");

// Start the conversation with context for the AI model
List<ChatMessage> chatHistory = new()
    {
        new ChatMessage(ChatRole.System, """
            You are a friendly travel agent who helps people discover picnic spots in their area.
            You introduce yourself when first saying hello.
            When helping people out, you always ask them for this information
            to inform the hiking recommendation you provide:

            1. What kind of picnic spots they are looking for 
            2. The location where they would like to visit

            You will then provide three suggestions for nearby picnic spots that they can vist
            after you get that information. You will also share an interesting fact about
            the local nature on the spots when making a recommendation. At the end of your
            response, ask if there is anything else you can help with.
        """)
    };

while (true)
{
    // Get user prompt and add to chat history
    Console.WriteLine("Your prompt:");
    var userPrompt = Console.ReadLine();
    chatHistory.Add(new ChatMessage(ChatRole.User, userPrompt));

    // Stream the AI response and add to chat history
    Console.WriteLine("AI Response:");
    var response = "";
    await foreach (var item in
        client.GetStreamingResponseAsync(chatHistory))
    {
        Console.Write(item.Text);
        response += item.Text;
    }
    chatHistory.Add(new ChatMessage(ChatRole.Assistant, response));
    Console.WriteLine();
}
