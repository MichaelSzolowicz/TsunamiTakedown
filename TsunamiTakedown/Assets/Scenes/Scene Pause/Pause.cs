using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class Pause : MonoBehaviour
{
    public static bool gamepause = false;
    void Update()
    {
        if (Input.GetKeyDown("Escape"))
        {
            gamepause = !gamepause;
            if (gamepause)
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1;
            }
        }
    }
}
