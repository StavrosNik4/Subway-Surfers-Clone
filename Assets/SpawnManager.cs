using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    public GameObject obstaclePrefab;
    public GameObject obstacle2Prefab;
    public GameObject rewardPrefab;

    private Vector3 obstacleSpawnPos = new Vector3(60.0f, 0.0f, 0.0f);
    private Vector3 rewardSpawnPos = new Vector3(60.0f, 0.4f, 0.0f);
    private Vector3 obstacle2SpawnPos = new Vector3(60.0f, 2.6f, 0.0f);

    private float obstacleStartDelay = 1.5f;
    private float obstacleRepeatDelay = 2.0f;

    private float rewardStartDelay = 5.0f;
    private float rewardRepeatDelay = 7.0f;

    private PlayerController playerControllerScript;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnObstacle", obstacleStartDelay, obstacleRepeatDelay);
        InvokeRepeating("SpawnReward", rewardStartDelay, rewardRepeatDelay);
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnObstacle()
    {
        if (playerControllerScript.gameOver == false)
        {
            // Generate a random number between 0 and 1
            int randomIndex = Random.Range(0, 2);

            // Use a switch statement to select which prefab to spawn based on the random number
            switch (randomIndex)
            {
                case 0:
                    Instantiate(obstaclePrefab, obstacleSpawnPos, obstaclePrefab.transform.rotation);
                    break;
                case 1:
                    Instantiate(obstacle2Prefab, obstacle2SpawnPos, obstacle2Prefab.transform.rotation);
                    break;
            }
        }
    }

    void SpawnReward()
    {
        if (playerControllerScript.gameOver == false)
        {
            Instantiate(rewardPrefab, rewardSpawnPos, rewardPrefab.transform.rotation);
        }
    }
}
