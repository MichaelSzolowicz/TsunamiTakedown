using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wave_movement : MonoBehaviour
{
    public float speed = 10;
    public int lifetime = 20;
    public bool changeDirection = false;
    private float lifecount;
    void Start()
    {
        this.gameObject.AddComponent<MeshCollider>();
        lifecount = 0;
    }
    
    void Update()
    {
        lifecount += Time.deltaTime;
        if (!changeDirection)
        {
            transform.position += new Vector3(1, 0, 0) * Time.deltaTime * speed;
        }
        else
        {
            transform.position += new Vector3(-1, 0, 0) * Time.deltaTime * speed;
        }
        
        if(lifecount > lifetime)
        {
            Destroy(this.gameObject);
        }
    }
}
