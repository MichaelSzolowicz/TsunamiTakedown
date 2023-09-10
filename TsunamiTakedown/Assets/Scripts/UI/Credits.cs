using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*Author: Shawn Bradbury
 * Date: 09/09/2023
 * This script will just let the player return to title when ready
 */
public class Credits : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            SceneManager.LoadScene(0);
        }
    }
}
