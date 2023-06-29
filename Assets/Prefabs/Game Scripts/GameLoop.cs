using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RegressionGames;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameLoop : MonoBehaviour
{

    public GameObject powerUpPrefab;
    public GameObject platform;

    void Awake()
    {
        RGSettings rgSettings = RGSettings.GetOrCreateSettings();
        if (rgSettings.GetUseSystemSettings())
        {
            int[] botIds = rgSettings.GetBotsSelected().ToArray();
            int errorCount = 0;
            Task.WhenAll(botIds.Select(botId =>
                RGServiceManager.GetInstance()
                    ?.QueueInstantBot((long)botId, (botInstance) => { }, () => errorCount++)));
            if (errorCount > 0)
            {
                Debug.Log($"Error starting {errorCount} of {botIds.Length} RG bots, starting without them");
            }
        }
        RGBotServerListener.GetInstance()?.StartGame();
    }

    private void OnDestroy()
    {
        RGBotServerListener.GetInstance()?.StopGame();
    }

    // Update is called once per frame
    void Update()
    {
        // If there are no power ups available, create a new one at a random position

        if (FindObjectOfType<PowerUp>() == null)
        {
            var middleOfPlatform = platform.transform.position;
            var spawnPosition = new Vector3(
                middleOfPlatform.x + Random.Range(0, 10) - 5,
                middleOfPlatform.y + 5,
                middleOfPlatform.z + Random.Range(0, 10) - 5
            );
            Instantiate(powerUpPrefab, spawnPosition, Quaternion.identity);
        }
        
    }
}
