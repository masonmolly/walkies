using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    /*
     The PauseMenu script is attached to the pause menu UI within the hub scene. It is responsible for determining whether and how to pause the game, and what actions to take when the resume or quit buttons are pressed within the pause UI.
    */

    public static bool pause;
    [SerializeField] //serialized field to hold the pause UI game object
    GameObject pauseUI;
    PlayerController viewMode;

    // Start is called before the first frame update
    void Start()
    {
        pause = false; //ensures game isn't paused on runtime
        viewMode = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
       if ((Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape)) && BulletinUI.isBulletinOn == false && viewMode.viewMode == false) //if escape or p key are pressed, and not in view mode or bulletin on, game is paused if not already paused; freezing game time and turning on the pause UI
        {
            if (pause == false) 
            {
                pause = true; 
                pauseUI.SetActive(true);
                Time.timeScale = 0.0f; 
            }
            else //if game is paused, run resume button function; reverts pause changes by turning off the pause UI, and unfreezing game time
            {
                resumeButton();
            }
        }
    }

    public void resumeButton() //function applied to resume button, also runs if the game is paused and the player presses escape or p key. Reverts changes made when game is paused
    {
        pause = false;
        pauseUI.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void quitButton() //function applied to quit button, quits the application upon click
    {
        Application.Quit();
    }
}
