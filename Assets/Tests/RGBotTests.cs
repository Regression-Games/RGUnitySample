using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using RegressionGames;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class RGBotTests
{
    [UnityTest]
    public IEnumerator RunBotTest()
    {
        string botId = "1000010";//Environment.GetEnvironmentVariable("RG_BOT");
        string rgApiKey = "something"; //Environment.GetEnvironmentVariable("RG_API_KEY");
        
        AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync("Scenes/SampleScene", LoadSceneMode.Single);
        // Wait until the level finish loading
        while (!asyncLoadLevel.isDone)
            yield return null;
        // Wait a frame so every Awake and Start method is called
        yield return new WaitForEndOfFrame();

        // Assert.AreEqual(botId, "32");
        // Assert.AreEqual(rgApiKey, "ABC");
        // Start the bot
        int[] botIds = {Int32.Parse(botId)};
        int errorCount = 0;
        // Let the test run
        RGBotServerListener.GetInstance().StartGame();
        Task.WhenAll(botIds.Select(delegate(int botId)
        {
            Debug.Log("Creating task to spawn bot with ID " + botId);
            return RGServiceManager.GetInstance()
                .QueueInstantBot((long) botId, (botInstance) =>
                {
                    RGBotServerListener.GetInstance().AddClientConnectionForBotInstance(botInstance.id);
                }, () => errorCount++);
        }));
        if (errorCount > 0)
        {
            Debug.LogError($"Error starting {errorCount} of {botIds.Length} RG bots");
        }
        RGBotServerListener.GetInstance().SpawnBots();
        
        var startTime2 = DateTime.Now;
        while (DateTime.Now.Subtract(startTime2).TotalSeconds < 10)
            yield return null;
        
        RGBotServerListener.GetInstance()?.StopGame();
        
    }
    
}
