using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Author: Bradbury, Shawn
 * Date: 09/10/2023
 * This Script will randomly spawn items after X amount of time
 */
public class Spawner : MonoBehaviour
{
    [SerializeField]
    private float timer;

    [SerializeField]
    private GameObject leftBound;

    [SerializeField]
    private GameObject rightBound;

    [SerializeField]
    GameObject[] powerUpChoices;

    /// <summary>
    /// Start the process of calling SpawnItems
    /// </summary>
    void Start()
    {
        Invoke("spawnItems", timer);
    }
    
    /// <summary>
    /// Spawns random Item at a random time then invokes itself after timeseconds to do so again
    /// </summary>
    public void spawnItems()
    {
        int itemChoice = Random.RandomRange(0, 3);
        float spawnX = Random.Range (leftBound.transform.position.x, rightBound.transform.position.x);

        GameObject powerUp = Instantiate(powerUpChoices[itemChoice]);
        powerUp.transform.position = new Vector3 (spawnX, rightBound.transform.position.y, transform.position.z);

        Invoke("spawnItems", timer);
    }
}
