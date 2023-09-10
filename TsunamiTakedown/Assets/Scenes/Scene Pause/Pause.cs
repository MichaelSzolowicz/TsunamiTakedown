using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class Pause : MonoBehaviour
{
    public static bool gamepause = false;
    public GameObject PauseMenu;
    private void Start()
    {
        PauseMenu.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gamepause = !gamepause;
            if (gamepause)
            {
                PauseMenu.SetActive(true);
                Time.timeScale = 0f;
            }
            else
            {
                PauseMenu.SetActive(false);
                Time.timeScale = 1;
            }
        }
    }
}
