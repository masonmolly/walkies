using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelPlayerController : MonoBehaviour
{
    /*
     The LevelPlayerController script is attached to the player game object within all of the levels. It is responsible for moving the player in the levels, triggering the level end, playing collision audio, and updating the distance score.
    */

    float moveSpeed;
    public static int lives; //public static allows the distance UI to access the variable for live visible updating
    public static int difficulty; //public static allows variable to be updated upon accessing a level in different scene (hub)
    float distance;
    public static int distanceInt; //public static allows the distance UI to access the variable for live visible updating
    public static bool audioPlayObstacle = false;
    public static bool audioPlayPowerUp = false;
    AudioSource obstacleAudio, powerUpAudio;

    [SerializeField]
    GameObject levelOverUI; //var to hold the UI for the level over
    bool pause;

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 10.0f;
        lives = 3; //starts the player off with 3 lives
        distance = 0.0f; //starts the player off with a score of 0
        difficulty = PlayerController.difficulty; //gets the level difficulty from the hub player object

        obstacleAudio = GameObject.Find("ObstacleAudio").GetComponent<AudioSource>(); //sources audio
        powerUpAudio = GameObject.Find("PowerUpAudio").GetComponent<AudioSource>();

        switch (difficulty) //changes player speed dependent on level difficulty (higher levels = slower player)
        {
            case 1:
                moveSpeed = 3.0f;
                break;
            case 2:
                moveSpeed = 2.5f;
                break;
            case 3:
                moveSpeed = 1.5f;
                break;
            case 4:
                moveSpeed = 1.3f;
                break;
        }

    }

    // Update is called once per frame
    void Update()
    {
        pause = LevelPauseMenu.pause; //gets the pause status from the LevelPauseMenu script

        //LEVEL SCORE
        if (pause == false && lives > 0) //if statement updates distance score (if game isn't paused and player has lives)
        {
            distance += 0.1f;
        }
        distanceInt = Mathf.RoundToInt(distance); //makes distance readable; integer form

        //PLAYER MOVEMENT
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) //move player left
        {         
            transform.Translate(Vector3.left * Time.deltaTime * moveSpeed);       
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) //move player right
        {
            transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
        }

        //COLLISION AUDIO
        if (audioPlayObstacle == true) //trigger to play obstacle audio - bool variable is changed to true in SpawnBehaviour script
        {
            obstacleAudio.Play();
            audioPlayObstacle = false; //resets variable to allow for replay
        }
        else if (audioPlayPowerUp == true) //trigger to play powerup audio - bool variable is changed to true in SpawnBehaviour script
        {
            powerUpAudio.Play();
            audioPlayPowerUp = false; //resets variable to allow for replay
        }

        //LEVEL OVER
        if (lives <= 0) //if player has 0 lives (or less), level ends by bringing up the level over UI
        {
            levelOverUI.SetActive(true);
        }       
    }


}
