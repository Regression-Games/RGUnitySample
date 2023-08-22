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
        
        // For in-editor purposes, feel free to define a default bot to use!
        int botId = 0;

        // NOTE: Make sure to fill in the name of the scene to start your test with!
        AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync("Scenes/SampleScene", LoadSceneMode.Single);
        // Wait until the scene finishes loading, then wait a frame so every Awake and Start method is called
        while (!asyncLoadLevel.isDone)
            yield return null;
        yield return new WaitForEndOfFrame();

        // Grab the bot to start (override with the one from CI/CD if defined)
        if (RGEnvConfigs.ReadBotId() != null)
        {
            botId = Int32.Parse(RGEnvConfigs.ReadBotId());
        }
        int[] botIds = {botId};
        Debug.Log($"Running test with bots {string.Join(", ", botIds)}");
        
        // Start the bots
        int errorCount = 0;
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
        
        // Wait until at least one bot is connected
        while (!RGBotServerListener.GetInstance().HasBotsRunning())
            yield return null;
        
        // Now run until all bots complete their tasks
        while (RGBotServerListener.GetInstance().HasBotsRunning())
            yield return null;
        
        // Cleanup when done
        RGBotServerListener.GetInstance()?.StopGame();
    }
    
}
