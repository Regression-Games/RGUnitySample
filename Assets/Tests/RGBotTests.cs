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

    private string timeNow()
    {
        return $"{DateTime.Now:yyyy-MM-ddTHH:mm:ss:ffff}  [{Thread.CurrentThread.ManagedThreadId}] --- ";
    }
    [UnityTest]
    public IEnumerator RunBotTest()
    {
        
        Environment.SetEnvironmentVariable("RG_API_KEY", "fef15243-8ab4-478f-8aeb-2e05277a7a2d");
        
        Debug.Log($"{timeNow()} Starting test");

        // Override this to change how long a test will wait for bots to join before failing
        const int TIMEOUT_IN_SECONDS = 60;

        // For in-editor purposes, feel free to define a default bot to use!
        int defaultBotId = 109;

        // NOTE: Make sure to fill in the name of the scene to start your test with!
        Debug.Log($"{timeNow()} Waiting for scene to load...");
        AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync("Scenes/SampleScene", LoadSceneMode.Single);
        // Wait until the scene finishes loading, then wait a frame so every Awake and Start method is called
        while (!asyncLoadLevel.isDone)
            yield return null;
        yield return new WaitForEndOfFrame();
        Debug.Log($"{timeNow()} Scene loaded");

        // Grab the bot to start (override with the one from CI/CD if defined)
        if (RGEnvConfigs.ReadBotId() != null)
        {
            defaultBotId = Int32.Parse(RGEnvConfigs.ReadBotId());
        }
        int[] botIds = {defaultBotId};
        Debug.Log($"{timeNow()} Loaded config, using bots {string.Join(", ", botIds)}");
        
        // do this before the queue
        RGBotServerListener.GetInstance().StartGame();
        
        // Start the bots (note that the unity web request needs the main thread, so we can't tie it up)
        // thus... we don't do this async, we wait for each one
        foreach (var botId in botIds)
        {
            Debug.Log(
                $"{timeNow()} Running task to spawn bot with ID: {botId}");
            var task = RGServiceManager.GetInstance()
                .QueueInstantBot((long) botId, (botInstance) =>
                {
                    RGBotServerListener.GetInstance().AddClientConnectionForBotInstance(botInstance.id);
                }, () =>
                {
                    Debug.LogError($"{timeNow()} Error starting bot with ID {botId}");
                });
            Debug.Log($"{timeNow()} Waiting for bot ID: {botId} to be queued...");
            yield return new WaitUntil(() => task.IsCompleted);
            if (!task.IsCompletedSuccessfully)
            {
                Debug.LogWarning($"{timeNow()} Error running task to queue bot id: {botId}\r\n" +
                                 $"Status: {task.Status}\r\n" +
                                 $"AsyncState: {task.AsyncState}\r\n" +
                                 $"Completed: {task.IsCompleted}\r\n" +
                                 $"Canceled: {task.IsCanceled}\r\n" +
                                 $"Faulted: {task.IsFaulted}\r\n" +
                                 $"Exception: {task.Exception}");
            }
        }
        
        
        
        Debug.Log($"{timeNow()} All bots queued!");
        
        // Wait until at least one bot is connected. Fail the test if the connection takes too long
        Debug.Log($"{timeNow()} Waiting for bots to connect...");
        var startTime = DateTime.Now;
        yield return new WaitUntil(() =>RGBotServerListener.GetInstance().HasBotsRunning() ||
                   (DateTime.Now.Subtract(startTime).TotalSeconds > TIMEOUT_IN_SECONDS));

        if (!RGBotServerListener.GetInstance().HasBotsRunning())
        {
            RGBotServerListener.GetInstance()?.StopGame();
            Assert.Fail($"{timeNow()} Bots failed to connect within {TIMEOUT_IN_SECONDS} seconds");
        }
        
        Debug.Log($"{timeNow()} Bots connected! Letting them run...");
        RGBotServerListener.GetInstance().SpawnBots();
        // Now run until all bots complete their tasks
        yield return new WaitUntil(() => !RGBotServerListener.GetInstance().HasBotsRunning());
        
        Debug.Log($"{timeNow()} Test finished! Cleaning up");
        
        // Cleanup when done
        RGBotServerListener.GetInstance()?.StopGame();
    }
    
}
