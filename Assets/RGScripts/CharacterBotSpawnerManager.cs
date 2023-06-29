using RegressionGames;
using UnityEngine;

public class CharacterBotSpawnerManager : RGBotSpawnManager
{
    
    [SerializeField]
    [Tooltip("The character to spawn")]
    private GameObject rgBotPrefab;

    [SerializeField]
    [Tooltip("Spawn point for RG Bots")]
    private Transform botSpawnPoint;

    public override GameObject GetBotPrefab()
    {
        return rgBotPrefab;
    }

    public override Transform GetBotSpawn()
    {
        return botSpawnPoint;
    }

    public override GameObject SpawnBot(bool lateJoin, uint clientId, string botName, string characterConfig)
    {
        Debug.Log($"LATE JOIN ${lateJoin}");
        GameObject spawnedBot = base.SpawnBot(lateJoin, clientId, botName, characterConfig);
        // Do something special, like configuration, creating a networked object, etc...

        RGPlayerMoveAction moveAction = spawnedBot.GetComponent<RGPlayerMoveAction>();
        BotCharacterConfig config = JsonUtility.FromJson<BotCharacterConfig>(characterConfig);
        if (config != null)
        {
            Debug.Log($"Changed speed to ${config.speed}");
            moveAction.speed = config.speed;
        }
        return spawnedBot;
    }

    public override void TeardownBot(uint clientId)
    {
        base.TeardownBot(clientId);
        // Now do something special, like cleaning network information or cleaning up state.
    }
}
