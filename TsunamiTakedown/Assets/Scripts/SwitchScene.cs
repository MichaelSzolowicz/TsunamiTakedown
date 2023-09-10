using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour
{
    public void change_button()
    {
        SceneManager.LoadScene(1);
        Debug.Log("button pressed");
    }
}
