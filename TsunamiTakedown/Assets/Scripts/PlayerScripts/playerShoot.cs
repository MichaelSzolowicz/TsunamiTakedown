using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerShoot : MonoBehaviour
{
    [SerializeField]
    private GameObject torpPrefab;

    //Player States: Aim, Readying, Firing, Recovering
    public string playerState;

    // Start is called before the first frame update
    void Start()
    {
        playerState = "Aim";
    }

    /// <summary>
    /// Run the script based on current state each second
    /// </summary>
    private void FixedUpdate()
    {
        stateManaging();
    }

    /// <summary>
    /// Choose which state action to run based on player's current state
    /// </summary>
    public void stateManaging()
    {
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
    }

    /// <summary>
    /// Lock the cursor, aim the ship towards the targeting cursor's clicked position 
    /// </summary>
    public void Readying()
    {
        //
    }

    /// <summary>
    /// Instantiate and fire a torpedo towards the cursor's clicked position
    /// </summary>
    public void Shooting()
    {
        //
    }

    /// <summary>
    /// Reset the ship's rotation to be back to its normal position then reset back to Aiming
    /// </summary>
    public void Recovering()
    {

    }
}
