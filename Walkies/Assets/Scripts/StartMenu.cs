using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    /*
     The StartMenu script is attached to the start menu in the start scene. It is responsible for determining what to to when the buttons within the start UI are clicked.
    */

    [SerializeField]
    GameObject fadeIn; //allows for reference to fade in animation object

    float timer;
    bool timerOn;
    public static bool firstGame = true; //public static to hold game state of whether the player has loaded the hub before or not (and thus to inform the scene whether the introduction text should show or not)

    // Start is called before the first frame update
    void Start()
    {
        timerOn = false; //ensures timer is reset
        timer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerOn == true) //timer starts when true (thus when start button is pressed)
        {
            timer += Time.deltaTime;
            if (timer >= 2.0f) //loads hub scene once a few seconds have passed after pressing the start button (allowing time for the fade in animation to complete)
            {
                SceneManager.LoadScene("Hub", LoadSceneMode.Single);
            }
        }
        
    }

    public void startButton() //function that is applied to the start button
    {
        fadeIn.SetActive(true); //fadeIn animation activates once start button is pressed
        timerOn = true;   //starts timer, to allow time for fade animation to run rather than immediately loading the hub scene
    }

    public void quitButton() //function that is applied to the quit button; quits application upon click
    {
        Application.Quit();
    }
}
