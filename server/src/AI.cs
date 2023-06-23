using OpenAI;
using OpenAI.Managers;
using OpenAI.ObjectModels;
using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Wrapper for the OpenAI package
/// </summary>
public static class AI
{

    static OpenAIService aiService;

    public static void Init()
    {
        Utils.Log("Initializing AI...");

        aiService = new OpenAIService(new OpenAiOptions()
        {
            ApiKey = Env.instance.openAIKey,
            DefaultModelId = Config.AI.DEFAULT_MODEL
        });

        Utils.Log("AI initialized");
    }

    public static async Task<string> Prompt(List<ChatMessage> messages)
    {
        try
        {
            Utils.Log("Sending prompt to AI...");

            ChatCompletionCreateResponse response = await aiService.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
            {
                Messages = messages,
                MaxTokens = Config.AI.MAX_RETURN_TOKENS,
            });

            Utils.Log($"Received AI response. Tokens Used: {response.Usage.TotalTokens}" +
                $"\n\tPrompt Tokens: {response.Usage.PromptTokens}" +
                $"\n\tCompletion Tokens: {response.Usage.CompletionTokens}" +
                $"\n\tCost: ${response.Usage.TotalTokens * Config.AI.COST_PER_TOKEN}");

            if (response.Successful)
            {
                return response.Choices.First().Message.Content;
            }
            else
            {
                if (response.Error != null)
                    Utils.Log($"AI Error: {response.Error.Message}");
                else
                    Utils.Log("AI Error: Unknown");

                return Utils.Style("Encountered an error with AI.", "red");
            }
        } catch (Exception e)
        {
            Utils.Log($"Error: {e.Message}\n{e.StackTrace}");
            return Utils.Style("Encountered an error with AI.", "red");
        }
    }

    public static async Task<string> Prompt(string prompt)
    {
        return await Prompt(new List<ChatMessage>()
        {
            new("user", prompt)
        });
    }

    public static async void FlavorMessage(Creature creature)
    {
        List<ChatMessage> messages = new()
        {
            new("user", $"Create a one sentence status message for Zombie 2 in a medieval game"),
            new("assistant", $"Zombie 2 groans"),
            new("user", $"Create a one sentence status message for {creature.name}")// in a medieval game")
        };

        //ContinueWith runs the code in the lambda after the task is finished
        await Prompt(messages).ContinueWith((task) =>
        {
            try
            {
                Player[] players = creature.Location.Players;

                string msg = task.Result.Replace(creature.name, creature.FormattedName); //Replace the name of the creature with the actual name

                foreach (Player p in players)
                    p.session?.Log(msg); //Send to all players in location
            } catch { }
        });
    }

}