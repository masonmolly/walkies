using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelPauseMenu : MonoBehaviour
{
    /*
     The LevelPauseMenu script is attached to the pause menu UI within the levels. It is responsible for determining when to pause the level, as well as determining the action to take when either the resume or return to hub buttons are pressed.
    */

    public static bool pause; //public static variable holds pause state of level for reference
    [SerializeField] //serialized field to hold the pause UI game object for the levels
    GameObject levelPauseUI;

    [SerializeField]
    GameObject fadeIn;
    float timer;
    bool timerOn;

    [SerializeField] //serialized field to hold footsteps audio
    GameObject footsteps;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0.0f; //ensures timer is reset
        timerOn = false;
        pause = false; //ensures the game is not paused at runtime
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape)) && LevelOverMenu.isLevelOver == false) //if the escape key or p is pressed, and the game is not currently paused or level over, pause state is set to true; turning off footstep audio, freezing game time, and activating the pauseUI. 
        {
            if (pause == false)
            {
                pause = true;
                footsteps.SetActive(false);
                levelPauseUI.SetActive(true);
                Time.timeScale = 0.0f;
            }
            else //if the game is currently paused, the previous changes are reversed by running the resume function (thus game is unpaused)
            {
                resumeButton();
            }
        }

        if (timerOn == true) //timer is true when return to hub button is pressed, loads hub scene after a few seconds have passed (enough time for fade in animation to play)
        {
            timer += Time.deltaTime;
            if (timer >= 5.0f)
            {
                SceneManager.LoadScene("Hub", LoadSceneMode.Single);
            }
        }
    }

    public void resumeButton() //function applied to resume button, is also used if the escape or p key is pressed while game is currently pause. Removes pause UI, turns back on footstep audio, and un-freezes game time
    {
        pause = false;
        footsteps.SetActive(true);
        levelPauseUI.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void returnToHubButton() //function applied to the return to hub button, triggers the timer on to load the hub scene after a few seconds. Unfreezes time for next scene
    {              
        timerOn = true;
        fadeIn.SetActive(true);
        Time.timeScale = 1.0f;
    }
}