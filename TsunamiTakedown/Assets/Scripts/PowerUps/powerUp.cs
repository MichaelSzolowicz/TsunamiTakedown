using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Author: Bradbury, Shawn
 * Date: 09/10/2023
 * This Script will let the items Gently fall down and then self destruct after some time
 */
public class powerUp : MonoBehaviour
{
    [SerializeField]
    private float fallSpeed;

    [SerializeField]
    private float despawnTime;

    /// <summary>
    /// Call selfdestruct when the lifespan has been reached so it doesnt clog the background with powerups
    /// </summary>
    void Start()
    {
        Invoke("SelfDestruct", despawnTime);
    }

    private void FixedUpdate()
    {
        Vector3 tempPos = curPos;
        tempPos.y -= fallSpeed * Time.deltaTime;
        curPos = tempPos;
    }

    /// <summary>
    /// Sets transform.position as a variable temporarily
    /// </summary>
    public Vector3 curPos
    {
        get
        {
            return this.transform.position;
        }
        set
        {
            this.transform.position = value;
        }
    }

    /// <summary>
    /// When called self destruct
    /// </summary>
    public void SelfDestruct()
    {
        Destroy(this.gameObject);
    }
}
