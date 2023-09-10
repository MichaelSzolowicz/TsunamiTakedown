using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{

    public GameObject wave1;
    public GameObject wave2;
    public bool Switch = false;
    public float interval = 7;
    private float timecount = 0;
    public float width = 7;
    public float NumberSpawnFirst = 4;
    void Start()
    {
        for (int i = 0; i < NumberSpawnFirst; ++i)
        {
            if (!Switch)
            {
                Vector3 spawnpoint = this.transform.position;
                spawnpoint.x += width*i;
                Instantiate(wave1, spawnpoint, wave1.transform.rotation);
            }
            else
            {
                Vector3 spawnpoint = this.transform.position;
                spawnpoint.x += width*i;
                Instantiate(wave2, spawnpoint, wave1.transform.rotation);
            }
            Debug.Log(i);
        }
    }
    
    void Update()
    {
        timecount += Time.deltaTime;
        if(timecount > interval)
        {
            if (!Switch)
            {
                Vector3 spawnpoint = this.transform.position;
                Instantiate(wave1, spawnpoint, wave1.transform.rotation);
            }
            else
            {
                Vector3 spawnpoint = this.transform.position;
                Instantiate(wave2, spawnpoint, wave1.transform.rotation);
            }
            timecount = 0;
        }
        //Instantiate(wave1, this.transform.position, wave1.transform.rotation);
        //Instantiate(wave2, this.transform.position, wave2.transform.rotation);
    }
}
