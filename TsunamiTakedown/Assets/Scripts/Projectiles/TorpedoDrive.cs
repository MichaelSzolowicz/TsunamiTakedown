using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Author: Shawn Bradbury
 * Date: 09/09/2023
 * The job of this script is to push the torpedo forward, and explode it after x amount of time.
 */
public class TorpedoDrive : MonoBehaviour
{
    Rigidbody m_Rigidbody;

    [SerializeField]
    private float speed = 2.3f;

    public float burstForce = 500;

    private int timer = 200;

    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();

        m_Rigidbody.AddForce(transform.forward * burstForce);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        torpedoMove();
    }

    public void torpedoMove()
    {

        if (timer > 0)
        {
            timer--;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "SeaMonster")
        {
            timer = 0;
        }
    }
}
