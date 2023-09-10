using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerStats : MonoBehaviour
{
    [SerializeField]
    private int maxHP;

    [SerializeField]
    private int hp;

    private bool invincible = false;

    void Start()
    {
        hp = maxHP;
    }

    void removeInvincible()
    {
        invincible = false;
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "SeaMonster" && invincible == false)
        {
            hp--;
            invincible = true;
            Invoke("removeInvincible", 2f);
        }
    }
}
