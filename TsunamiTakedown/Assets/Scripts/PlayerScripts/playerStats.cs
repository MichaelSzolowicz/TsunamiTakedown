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

    void Start()
    {
        hp = maxHP;
    }

    void RemoveInvincible()
    {
        invincible = false;
    }

    void GameLost()
    {
        gameOverUI.SetActive(true);
        Time.timeScale = 0f;
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "SeaMonster" && invincible == false)
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
}
