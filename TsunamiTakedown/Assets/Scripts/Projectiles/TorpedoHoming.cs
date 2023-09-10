using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorpedoHoming : MonoBehaviour
{
    Rigidbody m_Rigidbody;

    private GameObject seaMonster;

    [SerializeField]
    private float speed = 2.3f;

    public float burstForce = 500;

    private int timer = 200;

    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();

        m_Rigidbody.AddForce(transform.forward * burstForce);

        seaMonster = GameObject.FindGameObjectWithTag("SeaMonster");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        TorpedoMove();
    }

    public void TorpedoMove()
    {
        transform.LookAt(seaMonster.transform.position);
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
