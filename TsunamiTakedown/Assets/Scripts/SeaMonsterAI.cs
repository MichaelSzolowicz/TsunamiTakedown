using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    /*
     * Author: Bradbury, Shawn
     * Date: 09/08/2023
     * This script will let the Sea Monster have an AI 
     */
public class SeaMonsterAI : MonoBehaviour
{
    //Two variables for tracking the boundaries
    private Vector3 leftBound;
    private Vector3 rightBound;
    private Vector3 jumpHeight;

    //Variable for speed and direction
    public float speed = 6f;
    private float baseSpeed = 6f;
    private float maxSpeed = 23;

    //Variables for positions
    private Vector3 diveHeight;
    private Vector3 normalHeight;

    //Variables for positions
    public GameObject leftBoundCube;
    public GameObject rightBoundCube;
    public GameObject bottomBoundCube;
    public GameObject player;

    //Bools to check state of monster
    private Vector3 playerTargetPos;
    private Vector3 jumpStartPos;

    public int arcHeight;

    //Booleans
    private bool bounce = false;

    //State checker
    public string enemyState;

    //Checking which attack is being done
    private int attackType;
    
    // Start is called before the first frame update
    void Start()
    {
        //Set the standard height for the monster
        normalHeight = curPos;
        diveHeight = bottomBoundCube.transform.position;

        //Set the Bounds
        leftBound = leftBoundCube.transform.position;
        rightBound = rightBoundCube.transform.position;

        enemyState = "swimming";
    }

    private void FixedUpdate()
    {
        if (enemyState != "attacking" && enemyState != "falling")
        {
            speedUp();
        }       
        else
        {
            jumpAttack();
        }
    }

    /// <summary>
    /// Monster needs to go back and forth multiple times
    /// </summary>
    public void monsterSwitch()
    {
        //Check if monster is past the rightmost bound, and if so bounce them over
        if(this.transform.position.x > rightBound.x || this.transform.position.x < leftBound.x)
        {
            if (enemyState != "rushing")
            {
                Bounce();
            }

            else
            {
                //Start rush protocol here
            }
        }
    }

    /// <summary>
    /// Ping pongs the monster back and forth when they reach the edge of their movement
    /// </summary>
    public void Bounce()
    {
        //Check which way the sea-monster is currently going and then reset it
        if (bounce == false)
        {
            bounce = true;

            if (enemyState == "ready")
            {
                enemyState = "attacking";
                playerTargetPos = player.transform.position;
                jumpStartPos = curPos;
                arcHeight = Random.Range(10, 15);

                jumpHeight.x = (playerTargetPos.x + jumpStartPos.x) / 2;

                jumpHeight.y = playerTargetPos.y;
                jumpHeight.y += arcHeight;
            }
        }
        else
        {
            bounce = false;
        }
    }

    //Speed the Monster up a little as it goes back and forth
    public void speedUp()
    {
        Vector3 tempPos = curPos;

        if (enemyState != "recovering")
        {
            if (bounce == false)
            {
                tempPos.x += speed * Time.deltaTime;
            }
            else
            {
                tempPos.x -= speed * Time.deltaTime;
            }

            //Check if max speed is hit and dive
            if (enemyState == "diving" && this.transform.position.y > diveHeight.y)
            {
                tempPos.y -= speed / 4 * Time.deltaTime;

                if (tempPos.y <= diveHeight.y)
                {
                    enemyState = "ready";
                }
            }
            /*else if (enemyState == "swimming")
            {
                tempPos.y = curPos.y - (Mathf.Cos(Time.time * Mathf.PI * 1.25f))/.8f;
            }*/

        }

        else
        {
           if(tempPos.y > normalHeight.y)
            {
                tempPos.y -= speed*.7f * Time.deltaTime;
            }

           if(speed > baseSpeed)
            {
                speed--;
            }

            else if (speed <= baseSpeed && tempPos.y <= normalHeight.y)
            {
                enemyState = "swimming";
            }
        }

        

        curPos = tempPos;

        //Check speed
        if (enemyState != "attacking" && speed < maxSpeed)
        {
            speed += Random.Range(0.02f, 0.05f);

            if (speed >= maxSpeed)
            {
                attackType = Random.Range(1, 3);
                switch (attackType)
                {
                    case (1):
                        enemyState = "diving";
                        break;

                    case (2):
                        enemyState = "rushing";
                        break;

                    default:
                        enemyState = "readying";
                        break;
                }
            }
        }

        monsterSwitch();
    }

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
    /// function that does the math for an arcing jump attack
    /// </summary>
    public void jumpAttack()
    {
        if (enemyState == "falling")
        {
            Vector3 nextPos = Vector3.MoveTowards(transform.position, playerTargetPos, (speed * .83f) * Time.deltaTime);
            curPos = nextPos;

            if (curPos == playerTargetPos)
            {
                enemyState = "recovering";
            }
        }

        if(enemyState == "attacking")
        {
            Vector3 nextPos = Vector3.MoveTowards(transform.position, jumpHeight, (speed * 1.1f) * Time.deltaTime);
            curPos = nextPos;

            if (curPos == jumpHeight)
            {
                enemyState = "falling";
                playerTargetPos = player.transform.position;
            }

            if(speed > baseSpeed * 1.5)
            {
                speed-=.2f;
            }
        }
    }
}
