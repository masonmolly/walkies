using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelOverMenu : MonoBehaviour
{
    /*
     The LevelOverMenu script is attached to the level over menu UI within the levels. It is responsible for deciding what to do when the level is over, determining how many stars (if any) the player receives, and deciding what to do when the return to hub button is pressed.
    */

    [SerializeField]
    GameObject fadeIn;

    float timer;
    bool timerOn;
    int score;
    public static bool isLevelOver = false; //public static bool holds reference for LevelPauseMenu to know if game can be paused or not (only paused if level isn't over)

    [SerializeField] //serialized star fields hold the star game objects - used to show/hide the stars dependent on the player's score upon level over
    GameObject star1;
    [SerializeField]
    GameObject star2;
    [SerializeField]
    GameObject star3;

    [SerializeField] //serialized footsteps field to hold footsteps audio
    GameObject footsteps;
    [SerializeField] //serialized level over field holds the level over UI
    GameObject levelOver;

    // Start is called before the first frame update
    void Start()
    {
        isLevelOver = true;
        timerOn = false; //ensures timer is reset
        timer = 0.0f;
        footsteps.SetActive(false); //turns off footsteps audio as soon as the level ends
        levelOver.SetActive(true); //activates the level over UI as soon as the level ends
    }

    // Update is called once per frame
    void Update()
    {
        score = LevelPlayerController.distanceInt; //gets the final score from the LevelPlayerController script
        if (score >= 200)
        {
            star1.SetActive(true); //if score is 200 or over, player gets 1 star
                if (score >= 500)
                {
                    star2.SetActive(true); //if score is 500 or over, player gets 2 stars
                        if (score >= 1000)
                        {
                            star3.SetActive(true); //if score is 1000 or over, player gets 3 stars
                        }
                }
        } 

        if (timerOn == true) //timer is true once return to hub button is clicked
        {
            timer += Time.deltaTime;
            if (timer >= 2.0f) //loads hub scene once a few seconds have passed after pressing the start button (allowing time for the fade in animation to complete)
            {
                SceneManager.LoadScene("Hub", LoadSceneMode.Single);
            }
        }
    }

    public void returnToHubButton() //function applied to the return to hub button - turns on the fade in animation, and sets the timer on to trigger load of hub scene
    {
        fadeIn.SetActive(true);
        timerOn = true;
    }
        
}
