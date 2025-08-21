using OpenAI;
using OpenAI.Managers;
using OpenAI.ObjectModels.RequestModels;

//Models name are from huggingface.co. 
//In hugging face you can find these models using inference provides filter
//All models may not responed, I tried Llama, it did not respond.
//Also fireworks-ai is a provider, and same models with other providers may not respond. Means provider also matter
var models = new List<string>
{
    "deepseek-ai/DeepSeek-V3:fireworks-ai",
    "openai/gpt-oss-20b:fireworks-ai",

};


// Display available models
Console.WriteLine("Available models:");
for (int i = 0; i < models.Count; i++)
{
    Console.WriteLine($"{i + 1}. {models[i]}");
}

// Get user selection
int selectedIndex;
do
{
    Console.Write("\nSelect a model (enter number): ");
} while (!int.TryParse(Console.ReadLine(), out selectedIndex) ||
         selectedIndex < 1 ||
         selectedIndex > models.Count);

// Get the selected model
string selectedModel = models[selectedIndex - 1];
Console.WriteLine($"\nSelected model: {selectedModel}\n");

// Get user prompt
Console.Write("Enter your prompt: ");
string userPrompt = Console.ReadLine() ?? string.Empty;


var api = new OpenAIService(new OpenAiOptions()
{
    ApiKey = "[HUGGING_FACE_API_KEY]",
    BaseDomain = "https://router.huggingface.co/v1"
});


var completionRequest = new ChatCompletionCreateRequest
{
    Model = selectedModel,
    Messages = new List<ChatMessage>
    {
        ChatMessage.FromUser(userPrompt)
    },
    Stream = true // Enable streaming
};



await foreach (var completion in api.ChatCompletion.CreateCompletionAsStream(completionRequest))
{
    if (completion.Successful)
    {
        Console.Write(completion.Choices.FirstOrDefault()?.Message.Content);
    }
    else
    {
        Console.WriteLine($"Error: {completion.Error?.Message}");
    }
}

Console.ReadKey();


