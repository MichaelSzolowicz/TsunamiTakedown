using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    /*
     * Author: Bradbury, Shawn
     * Date: 09/08/2023
     * This script will set the MonsterStats like health 
     */
public class MonsterStats : MonoBehaviour
{
    public int healthPoints = 20;
    public int maxHP = 20;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Torpedo")
        {
            healthPoints--;

            if (healthPoints <= 0)
            {
                this.gameObject.GetComponent<SeaMonsterAI>().enemyState = "Dying";
            }
        }
    }
}
