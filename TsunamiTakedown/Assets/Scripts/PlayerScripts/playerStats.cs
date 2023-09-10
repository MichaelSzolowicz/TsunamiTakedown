using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Author: Shawn Bradbury
 * Date: 09/09/2023
 * The job of this script is to push the torpedo forward, and explode it after x amount of time.
 */
public class playerStats : MonoBehaviour
{
    [SerializeField]
    private int maxHP;

    [SerializeField]
    private int hp;

    [SerializeField]
    private GameObject gameOverUI;

    private bool invincible = false;

    //Powerups booleans
    public bool rapidFire = false;
    private bool rapidFireExtension = false;

    public bool invincibility = false;
    private bool invincibilityExtension = false;

    public bool homing = false;
    private bool homingExtension = false;

    void Start()
    {
        hp = maxHP;
    }

    /// <summary>
    /// Renoves invincible from player
    /// </summary>
    public void RemoveInvincible()
    {
        if (invincibilityExtension != true)
        {
            invincible = false;
        }
        else
        {
            invincibilityExtension = false;
        }
    }

    /// <summary>
    /// Remove RapidFire
    /// </summary>
    public void RemoveRapidFire()
    {
        if (rapidFireExtension != true)
        {
            rapidFire = false;
        }
        else
        {
            rapidFireExtension = false;
        }
    }

    /// <summary>
    /// Remove homing
    /// </summary>
    public void RemoveHoming()
    {
        if (homingExtension != true)
        {
            homing = false;
        }
        else
        {
            homingExtension = false;
        }
    }

    /// <summary>
    /// GameOver UI
    /// </summary>
    public void GameLost()
    {
        gameOverUI.SetActive(true);
        Time.timeScale = 0f;
    }

    /// <summary>
    /// This ontrigger enter tracks damage and powerups
    /// </summary>
    /// <param name="collision">The variable for what the player collides into</param>
    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "SeaMonster" && invincible == false)
        {
            if (invincibility != true)
            {
                hp--;
                invincible = true;
                Invoke("RemoveInvincible", 2f);

                if (hp <= 0)
                {
                    GameLost();
                }
            }  
        }

        if (collision.gameObject.tag == "rapidFire")
        {
            if(rapidFire == true)
            {
                rapidFireExtension = true;
            }

            rapidFire = true;
            Invoke("RemoveRapidFire", 5f);
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "HealingKit")
        {
            if (maxHP > hp)
            {
                Destroy(collision.gameObject);
                hp++;
            }
        }

        if (collision.gameObject.tag == "Invincibility")
        {
            if (invincibility == true)
            {
                invincibilityExtension = true;
            }
            invincibility = true;
            Invoke("RemoveInvincibility", 5f);
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "Homing")
        {
            if (homing == true)
            {
                homingExtension = true;
            }
            homing = true;
            Invoke("RemoveHoming", 5f);
            Destroy(collision.gameObject);
        }
    }
}
