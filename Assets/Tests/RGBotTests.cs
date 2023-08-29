using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using RegressionGames;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class RGBotTests
{
    [UnityTest]
    public IEnumerator RunBotTest()
    {
        Debug.Log($"{DateTime.Now:yyyy-MM-dd- HH:mm:ss:ffff} Starting test");

        // Override this to change how long a test will wait for bots to join before failing
        const int TIMEOUT_IN_SECONDS = 600;

        // For in-editor purposes, feel free to define a default bot to use!
        int defaultBotId = 109;

        // NOTE: Make sure to fill in the name of the scene to start your test with!
        Debug.Log($"{DateTime.Now:yyyy-MM-dd- HH:mm:ss:ffff} Waiting for scene to load...");
        AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync("Scenes/SampleScene", LoadSceneMode.Single);
        // Wait until the scene finishes loading, then wait a frame so every Awake and Start method is called
        while (!asyncLoadLevel.isDone)
            yield return null;
        yield return new WaitForEndOfFrame();
        Debug.Log($"{DateTime.Now:yyyy-MM-dd- HH:mm:ss:ffff} Scene loaded");

        // Grab the bot to start (override with the one from CI/CD if defined)
        if (RGEnvConfigs.ReadBotId() != null)
        {
            defaultBotId = Int32.Parse(RGEnvConfigs.ReadBotId());
        }
        int[] botIds = {defaultBotId};
        Debug.Log($"{DateTime.Now:yyyy-MM-dd- HH:mm:ss:ffff} Loaded config, using bots {string.Join(", ", botIds)}");
        
        // Start the bots
        RGBotServerListener.GetInstance().StartGame();
        Task.WhenAll(botIds.Select(delegate(int botId)
        {
            Debug.Log($"{DateTime.Now:yyyy-MM-dd- HH:mm:ss:ffff} Creating task to spawn bot with ID " + botId);
            return RGServiceManager.GetInstance()
                .QueueInstantBot((long) botId, (botInstance) =>
                {
                    RGBotServerListener.GetInstance().AddClientConnectionForBotInstance(botInstance.id);
                }, () =>
                {
                    Debug.LogError($"{DateTime.Now:yyyy-MM-dd- HH:mm:ss:ffff} Error starting bot with ID {botId}");
                });
        }));
        RGBotServerListener.GetInstance().SpawnBots();
        
        // Wait until at least one bot is connected. Fail the test if the connection takes too long
        Debug.Log($"{DateTime.Now:yyyy-MM-dd- HH:mm:ss:ffff} Waiting for bots to connect...");
        var startTime = DateTime.Now;
        while (!RGBotServerListener.GetInstance().HasBotsRunning())
        {
            var timePassed = DateTime.Now.Subtract(startTime).TotalSeconds;
            if (timePassed > TIMEOUT_IN_SECONDS)
            {
                RGBotServerListener.GetInstance()?.StopGame();
                Assert.Fail($"{DateTime.Now:yyyy-MM-dd- HH:mm:ss:ffff} Bots failed to connect within {TIMEOUT_IN_SECONDS} seconds");
            }
            // Cleanup just in case
            yield return null;
        }
        
        Debug.Log($"{DateTime.Now:yyyy-MM-dd- HH:mm:ss:ffff}Bots connected! Letting them run...");
        // Now run until all bots complete their tasks
        while (RGBotServerListener.GetInstance().HasBotsRunning())
            yield return null;
        
        Debug.Log($"{DateTime.Now:yyyy-MM-dd- HH:mm:ss:ffff} Test finished! Cleaning up");
        
        // Cleanup when done
        RGBotServerListener.GetInstance()?.StopGame();
    }
    
}
