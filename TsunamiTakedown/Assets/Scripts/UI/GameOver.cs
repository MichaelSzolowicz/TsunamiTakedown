using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

/*Author: Shawn Bradbury
 * Date: 09/09/2023
 * This script gives the gameover menu options for what to do when buttons are pressed
 */
public class GameOver : MonoBehaviour
{
    [SerializeField]
    private GameObject seaMonster;

    [SerializeField]
    private TMP_Text lossQuote;

    private int dragHP;
    private int dragMaxHP;

    void Awake()
    {
        //Set these variables to make typing easier and legible
        dragHP = seaMonster.GetComponent<MonsterStats>().healthPoints;
        dragMaxHP = seaMonster.GetComponent<MonsterStats>().maxHP;

        if (dragHP <= dragMaxHP / 2)
        {
            if(dragHP <= dragMaxHP / 4)
            {
                lossQuote.text = "So close, the leviathan limped away with " + dragHP + " health!"; 
            }

            else
            {
                lossQuote.text = "That leviathan's a tough one, they scampered away with " + dragHP + " health.";
            }
        }

        else
        {
            lossQuote.text = "A prepared and worthy predator, the leviathan takes its meal with " + dragHP + " health.";
        }
    }

    /// <summary>
    /// Reloads scene so player can try again
    /// </summary>
    public void TryAgain()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// Returns player to main menu if clicked
    /// </summary>
    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Quite the user out of the game
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
}
