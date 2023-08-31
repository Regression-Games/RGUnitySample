using System;
using System.Collections;
using System.Linq;
using System.Threading;
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
        
        Environment.SetEnvironmentVariable("RG_API_KEY", "fef15243-8ab4-478f-8aeb-2e05277a7a2d");
        
        Debug.Log($"{DateTime.Now:yyyy-MM-dd- HH:mm:ss:ffff} Starting test");

        // Override this to change how long a test will wait for bots to join before failing
        const int TIMEOUT_IN_SECONDS = 60;

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
        var tasks = botIds.Select(botId =>
        {
            Debug.Log(
                $"{DateTime.Now:yyyy-MM-dd- HH:mm:ss:ffff} Creating task ({Thread.CurrentThread.ManagedThreadId}) to spawn bot with ID " +
                botId);
            return RGServiceManager.GetInstance()
                .QueueInstantBot((long) botId, (botInstance) =>
                {
                    RGBotServerListener.GetInstance().AddClientConnectionForBotInstance(botInstance.id);
                }, () =>
                {
                    Debug.LogError($"{DateTime.Now:yyyy-MM-dd- HH:mm:ss:ffff} Error starting bot with ID {botId}");
                });
        }).ToArray();
        foreach (var task in tasks)
        {
            task.RunSynchronously();
        }
        RGBotServerListener.GetInstance().SpawnBots();
        Debug.Log($"{DateTime.Now:yyyy-MM-dd- HH:mm:ss:ffff} Waiting for all bot start requests to send out...");
        var startTime1 = DateTime.Now;
        while (!tasks.All(t => t.IsCompleted))
        {
            var timePassed = DateTime.Now.Subtract(startTime1).TotalSeconds;
            if (timePassed > TIMEOUT_IN_SECONDS) {
                Debug.Log($"{DateTime.Now:yyyy-MM-dd- HH:mm:ss:ffff} Web request still waiting... terminating");
                Assert.Fail("Test failed because the web request for queueing a bot never completed");
            }
            yield return new WaitForFixedUpdate();
        }
        Debug.Log($"{DateTime.Now:yyyy-MM-dd- HH:mm:ss:ffff} All bot requests finished!");

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
            // Debug.Log($"WAITING FOR BOTS TO CONNECT... inside the loop in thread {Thread.CurrentThread.ManagedThreadId}");
            yield return new WaitForFixedUpdate();
        }
        
        Debug.Log($"{DateTime.Now:yyyy-MM-dd- HH:mm:ss:ffff} Bots connected! Letting them run...");
        // Now run until all bots complete their tasks
        while (RGBotServerListener.GetInstance().HasBotsRunning())
            yield return new WaitForFixedUpdate();
        
        Debug.Log($"{DateTime.Now:yyyy-MM-dd- HH:mm:ss:ffff} Test finished! Cleaning up");
        
        // Cleanup when done
        RGBotServerListener.GetInstance()?.StopGame();
    }
    
}
