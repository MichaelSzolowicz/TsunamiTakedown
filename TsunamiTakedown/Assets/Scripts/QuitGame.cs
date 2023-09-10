using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGame : MonoBehaviour
{
    public void QuitApplication()
    {
        UnityEditor.EditorApplication.isPlaying = false; //when Unity editor
        Application.Quit();//when application
    }
}
