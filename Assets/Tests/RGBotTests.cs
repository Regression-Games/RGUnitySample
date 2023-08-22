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
        
        // DEBUG: Set env vars for simulation
        Debug.Log("CUSTOM PARAMETERS");
        Debug.Log(Environment.GetEnvironmentVariable("CUSTOM_PARAMETERS"));
        Debug.Log("Command Line Args:" + string.Join("~~~", Environment.GetCommandLineArgs()));
        // Environment.SetEnvironmentVariable(RGEnvVars.RG_API_KEY, "33a213ec-04bf-42cb-b2b6-9f430856f766");
        // Environment.SetEnvironmentVariable(RGEnvVars.RG_HOST, "http://localhost:8080");
        // Environment.SetEnvironmentVariable(RGEnvVars.RG_BOT, "1000015");

        AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync("Scenes/SampleScene", LoadSceneMode.Single);
        // Wait until the level finish loading
        while (!asyncLoadLevel.isDone)
            yield return null;
        // Wait a frame so every Awake and Start method is called
        yield return new WaitForEndOfFrame();

        // Start the bot
        var providedBotId = Environment.GetEnvironmentVariable(RGEnvVars.RG_BOT);
        if (providedBotId == null)
        {
            Assert.Fail("No Bot ID given within env var 'RG_BOT' - please make sure to set this in your CI env");
        }
        int[] botIds = {Int32.Parse(providedBotId)};
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
        
        // First, wait until a bot is connected
        while (!RGBotServerListener.GetInstance().HasBotsRunning())
            yield return null;
        
        // Now run until all bots complete their tasks
        while (RGBotServerListener.GetInstance().HasBotsRunning())
            yield return null;
        
        RGBotServerListener.GetInstance()?.StopGame();
        
    }
    
}
