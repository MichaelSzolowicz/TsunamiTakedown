using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    [SerializeField]
    private GameObject leftBoundCube;

    [SerializeField]
    private GameObject rightBoundCube;

    [SerializeField]
    private GameObject bottomBoundCube;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private GameObject projectiles;

    //Bools to check state of monster
    private Vector3 playerTargetPos;
    private Vector3 jumpStartPos;

    private Quaternion baseRotation;
    private Quaternion secondRotation;

    public int arcHeight;

    //Booleans
    private bool bounce = false;

    //Checks dashcount
    private int dashCount;

    //State checker
    public string enemyState;

    //Checking which attack is being done
    public int attackType;
    
    // Start is called before the first frame update
    void Start()
    {
        //Set the standard height for the monster
        normalHeight = curPos;
        diveHeight = bottomBoundCube.transform.position;

        //Set the Bounds
        leftBound = leftBoundCube.transform.position;
        rightBound = rightBoundCube.transform.position;

        dashCount = 0;

        baseRotation = this.transform.rotation;
        secondRotation = baseRotation;
        secondRotation.y -= 180f;

        enemyState = "swimming";
    }

    private void FixedUpdate()
    {
        if (enemyState != "Dying")
        {
            if (enemyState != "attacking" && enemyState != "falling")
            {
                speedUp();
            }
            else
            {
                if (attackType == 1)
                {
                    jumpAttack();
                }
                else if (attackType == 2)
                {
                    rushAttack();
                }
            }
        }

        else
        {
            MonsterDeath();
        }
    }

    public void SpikeSpawning()
    {
        GameObject leftSpike = Instantiate(projectiles);
        Vector3 tempPos = leftSpike.transform.position;

        tempPos.x = Random.RandomRange(leftBound.x, transform.position.x);
        tempPos.y = rightBound.y;
        tempPos.z = 0;

        leftSpike.transform.position = tempPos;

        GameObject rightSpike = Instantiate(projectiles);
        tempPos.x = Random.RandomRange(transform.position.x, rightBound.x);

        rightSpike.transform.position = tempPos;
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
                playerTargetPos = player.transform.position;
                lungePrep();
            }
        }
    }

    /// <summary>
    /// Set the actions after ready into a function so it could be called for a second attack at lower hp
    /// </summary>
    public void readyAttack()
    {
        enemyState = "attacking";
        playerTargetPos = player.transform.position;
        jumpStartPos = curPos;
        arcHeight = Random.Range(10, 15);

        jumpHeight.x = (playerTargetPos.x + jumpStartPos.x) / 2;

        jumpHeight.y = playerTargetPos.y;
        jumpHeight.y += arcHeight;
    }

    /// <summary>
    /// Ping pongs the monster back and forth when they reach the edge of their movement
    /// </summary>
    public void Bounce()
    {
        //Check which way the sea-monster is currently going and then reset it
        if (bounce == false && curPos.x >= rightBound.x)
        {
            bounce = true;

            transform.rotation = secondRotation;

            if (enemyState == "ready")
            {
                readyAttack();
            }
        }
        else if (bounce == true && curPos.x <= leftBound.x)
        {
            transform.rotation = baseRotation;

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
                if (attackType == 1)
                {
                    if(this.GetComponent<MonsterStats>().healthPoints <= this.GetComponent<MonsterStats>().maxHP / 2)
                    {
                        SpikeSpawning();

                        //Check if health is below the threshold
                        if (this.GetComponent<MonsterStats>().healthPoints <= this.GetComponent<MonsterStats>().maxHP / 4)
                        {
                            //If still have a dash reset back to jumping again
                            if (dashCount == 0)
                            {
                                dashCount++;
                                speed = maxSpeed;
                                readyAttack();
                            }

                            else
                            {
                                enemyState = "swimming";
                                dashCount = 0;
                            }
                        }

                        else
                        {
                            enemyState = "swimming";
                        }
                    }

                    else
                    {
                        enemyState = "swimming";
                    }
                }

                else
                {
                    enemyState = "swimming";
                }
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

    public void lungePrep()
    {
        if(curPos.y < playerTargetPos.y)
        {
            Vector3 tempPos = curPos;
            tempPos.y += speed * .2f * Time.deltaTime;
            curPos = tempPos;
        }
        
        else
        {
            if (curPos.x < (leftBound.x - 14) || curPos.x > (rightBound.x + 14))
            {
                enemyState = "attacking";
                playerTargetPos = player.transform.position;

                if (bounce != true)
                {
                    bounce = true;
                    transform.rotation = secondRotation;
                }
                else
                {
                    bounce = false;
                    transform.rotation = baseRotation;
                }
            }   
        }
    }

    public void rushAttack()
    {
        Vector3 tempPos = curPos;
        if (enemyState == "attacking")
        {
            if (bounce == true)
            {
                if (leftBound.x + 2 < tempPos.x)
                {
                    tempPos.x -= speed * 1.2f * Time.deltaTime;
                }
                else
                {
                    /*if(this.GetComponent<MonsterStats>().healthPoints < this.GetComponent<MonsterStats>().maxHP / 2)
                    {
                        if(dashCount > 0)
                        {
                            dashCount = 0;
                            enemyState = "falling";
                        }
                        bounce = false;
                    }*/
                    enemyState = "falling";
                }
            }
            else
            {
                if (rightBound.x - 2 > tempPos.x)
                {
                    tempPos.x += speed * 1.2f * Time.deltaTime;
                }
                else
                {
                    enemyState = "falling";
                }
            }
            
        }

        if(enemyState == "falling")
        {
            if (speed > baseSpeed)
            {
                speed--;
            }

            if (bounce == true)
            {
                tempPos.x -= speed * 1.2f * Time.deltaTime;

                if(tempPos.x < leftBound.x)
                {
                    if (tempPos.y > normalHeight.y)
                    {
                        tempPos.y -= speed * Time.deltaTime;
                    }
                }

                else
                {
                    tempPos.y -= speed * .2f * Time.deltaTime;
                }
            }

            else if (bounce == false)
            {
                tempPos.x += speed * 1.2f * Time.deltaTime;

                if (tempPos.x > rightBound.x)
                {
                    if (tempPos.y > normalHeight.y)
                    {
                        tempPos.y -= speed * Time.deltaTime;
                    }
                }
                else
                {
                    tempPos.y -= speed * .2f * Time.deltaTime;
                }
            }

            if (speed <= baseSpeed && tempPos.y <= normalHeight.y)
            {
                if(this.GetComponent<MonsterStats>().healthPoints <= (this.GetComponent<MonsterStats>().maxHP / 2))
                {
                    switch (dashCount)
                    {
                        case 0:
                            enemyState = "rushing";
                            dashCount++;
                            speed = maxSpeed * 1.175f;
                            break;

                        case 1:
                            if(this.GetComponent<MonsterStats>().healthPoints <= (this.GetComponent<MonsterStats>().maxHP / 4)) 
                            {
                                enemyState = "rushing";
                                dashCount++;
                                speed = maxSpeed * 1.3f;
                                break;
                            }
                            else
                            {
                                enemyState = "recovering";
                                dashCount = 0;
                                break;
                            }

                        default:
                            enemyState = "recovering";
                            dashCount = 0;
                            break;
                    }
                }

                //Check to see the current state of the player
                else
                {
                    //Just recover
                    enemyState = "recovering";
                }
            }
        }

        curPos = tempPos;
    }
    
    /// <summary>
    /// cause the monster to dive then end the game
    /// </summary>
    public void MonsterDeath()
    {
        if (speed > baseSpeed)
        {
            speed = baseSpeed;
        }

        //Start by diving the monster down.
        Vector3 tempPos = curPos;
        tempPos.y -= speed * .5f * Time.deltaTime;
        curPos = tempPos;

        //Then once the monster is low enough, move to the victory scene
        if (curPos.y < diveHeight.y)
        { 
            SceneManager.LoadScene(2);
        }
    }
}
