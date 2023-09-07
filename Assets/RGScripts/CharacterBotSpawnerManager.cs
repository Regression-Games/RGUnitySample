using RegressionGames;
using RegressionGames.Types;
using UnityEngine;

public class CharacterBotSpawnerManager : RGBotSpawnManager
{
    
    [SerializeField]
    [Tooltip("The character to spawn")]
    private GameObject rgBotPrefab;

    [SerializeField]
    [Tooltip("Spawn point for RG Bots")]
    private Transform botSpawnPoint;

    public override GameObject SpawnBot(bool lateJoin, BotInformation botInformation)
    {
        Debug.Log("Spawning SAMPLE BOT");
        var bot = Instantiate(rgBotPrefab, Vector3.zero, Quaternion.identity);
        bot.transform.position = botSpawnPoint.position;

        BotCharacterConfig config = botInformation.ParseCharacterConfig<BotCharacterConfig>();
        
        // set player speed
        Player player = bot.GetComponent<Player>();
        if (config != null)
        {
            player.speed = config.speed;
        }

        return bot;
    }
    
}
