using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using RegressionGames;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.TestTools;

public class RGBotTests
{
    [UnityTest]
    public IEnumerator RunBotTest()
    {
        string botId = Environment.GetEnvironmentVariable("RG_BOT");
        string rgApiKey = Environment.GetEnvironmentVariable("RG_API_KEY");

        yield return new WaitForFixedUpdate();
        // Start RG
        RGBotServerListener.GetInstance()?.StartGame();
        Assert.AreEqual(botId, "32");
        Assert.AreEqual(rgApiKey, "ABC");
        // Start the bot
        int[] botIds = {Int32.Parse(botId)};
        int errorCount = 0;
        Task.WhenAll(botIds.Select(botId =>
            RGServiceManager.GetInstance()
                ?.QueueInstantBot((long)botId, (botInstance) => { }, () => errorCount++)));
        if (errorCount > 0)
        {
            Debug.Log($"Error starting {errorCount} of {botIds.Length} RG bots, starting without them");
        }
        // Let the test run
    }
    
}
