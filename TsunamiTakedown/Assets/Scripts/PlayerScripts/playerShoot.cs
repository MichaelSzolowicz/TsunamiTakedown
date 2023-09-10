using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


/*
* Author: Bradbury, Shawn
* Date: 09/08/2023
* This script will let the Sea Monster have an AI 
*/
public class playerShoot : MonoBehaviour
{
    [SerializeField]
    private GameObject torpPrefab;

    //[SerializeField]
    //private Texture2D cursorTexture;

    [SerializeField]
    private Camera _camera;

    public Quaternion rot;

    //Tracks cursor target for shooting
    private Vector3 cursorTarget;

    //Torpedo shot speed 8
    private float torpedoSS = 1f;

    private Quaternion ogRotation;

    //Player States: Aim, Readying, Firing, Recovering
    public string playerState;

    Controller controller;
    public RotParallelFloor rotator;

    // Start is called before the first frame update
    void Start()
    {
        AimingSetup();
        ogRotation = this.transform.rotation;

        controller = GetComponentInParent<Controller>();
    }

    /// <summary>
    /// Run the script based on current state each second
    /// </summary>
    private void Update()
    {
        StateManaging();
    }

    /// <summary>
    /// Choose which state action to run based on player's current state
    /// </summary>
    public void StateManaging()
    {
        Aiming();
        Readying();
        Shooting();

        return;

        if (playerState == "Aim")
        {
            Aiming();
        }

        else if (playerState == "Readying")
        {
            Readying();
        }

        else if (playerState == "Shooting")
        {
            Shooting();
        }

        else if (playerState == "Recovering")
        {
            Recovering();
        }
    }

    /// <summary>
    /// While player is aiming, replaces cursor with targeting cursor
    /// </summary>
    public void Aiming()
    {
        //

        //If the player left clicks, begin the shooting process.
        //if (Input.GetKey(KeyCode.Mouse0))
        {
            cursorTarget = mousePosition();
            //Cursor.lockState = CursorLockMode.Locked;
            if (this.gameObject.GetComponent<playerStats>().rapidFire == true)
            {
                torpedoSS = 20f;
            }
            else
            {
                torpedoSS = 5f;
            }
            
            playerState = "Readying";
        }
    }

    /// <summary>
    /// Lock the cursor, aim the ship towards the targeting cursor's clicked position 
    /// </summary>
    public void Readying()
    {
        //Start by rotating the player to face where they clicked
        //Cursor.lockState = CursorLockMode.Locked;

        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        //orbVector = Input.mousePosition - orbVector;
        //float angle = Mathf.Atan2(orbVector.y, orbVector.x) * Mathf.Rad2Deg;


        if(!rotator.probeHit)
        {
            transform.rotation = Quaternion.FromToRotation(Vector3.forward, (transform.position - cursorTarget).normalized);
            //print("Rot2: " + (transform.position - cursorTarget).normalized);
        }
        //Vector3 eul = transform.rotation.eulerAngles;



        /*
        Vector3 dir = cursorTarget - transform.position;
        dir.z = 0; // keep the direction strictly horizontal

        Quaternion rot = Quaternion.LookRotation(dir);

        // slerp to the desired rotation over time
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, torpedoSS * (torpedoSS*2) * Time.deltaTime);

        float angleCheck = Quaternion.Angle(transform.rotation, rot);
        //find a way to check when its done rotating so it can shoot
        if (angleCheck <= 1)
        {
            playerState = "Shooting";
        }
        */
    }

    /// <summary>
    /// Instantiate and fire a torpedo towards the cursor's clicked position
    /// </summary>
    public void Shooting()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            GameObject torp = Instantiate(torpPrefab);
            torp.transform.position = this.gameObject.transform.position;

            torp.transform.rotation = Quaternion.FromToRotation(Vector3.forward, (cursorTarget - transform.position).normalized);
            //torp.transform.rotation = Quaternion.Euler(this.GetComponent<playerMovement>().playerCamera.transform.eulerAngles.x, this.transform.eulerAngles.y, this.transform.eulerAngles.z);

            if(controller)
            {
                torp.GetComponent<Rigidbody>().velocity += controller.GetVelocity();
            }

            playerState = "Recovering";
        }
    }

    /// <summary>
    /// Reset the ship's rotation to be back to its normal position then reset back to Aiming
    /// </summary>
    public void Recovering()
    {
        /*
        // slerp to the desired rotation over time
        transform.rotation = Quaternion.Slerp(transform.rotation, ogRotation, torpedoSS * Time.deltaTime);
        
        
        float angleCheck = Quaternion.Angle(transform.rotation, ogRotation);
        //find a way to check when its done rotating so it can shoot
        if (angleCheck <= 1)
        {
            AimingSetup();
        }
        */
    }

    public void AimingSetup()
    {
        playerState = "Aim";
        //Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
    }
    
    /// <summary>
    /// Function to track the mouse position and return it as a Vector3 from world Space
    /// </summary>
    public Vector3 mousePosition()
    {
        Vector3 screenPosition = Input.mousePosition;

        screenPosition.z = transform.position.z - _camera.transform.position.z; //distance of the plane from the camera
        return Camera.main.ScreenToWorldPoint(screenPosition);
    }
}
