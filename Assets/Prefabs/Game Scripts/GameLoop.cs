using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameLoop : MonoBehaviour
{

    public GameObject powerUpPrefab;
    public GameObject platform;

    void Awake()
    {
        
    }

    private void OnDestroy()
    {
        
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
