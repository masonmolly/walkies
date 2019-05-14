using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoad : MonoBehaviour
{
    /*
     The LevelLoad script is attached to all of the level indicators in the hub scene. It is responsible for determining when to load the level scenes; if the player is within radius and holds down space, as well as which scene setting to load.
    */

    GameObject player;
    float keyHoldTime = 0.0f;
    string levelSetting;

    [SerializeField]
    GameObject fadeIn;
    float timer;
    bool timerOn;

    [SerializeField] //serialized field to hold music that plays when the player is about to start a level
    GameObject startMusic;

    void Start()
    {
        player = GameObject.Find("Player");
        timerOn = false;
        timer = 0.0f;
    }

    void Update()
    {
        float particleRadius = this.GetComponent<ParticleSystem>().shape.radius; 
        float distance = Vector3.Distance(this.transform.position, player.transform.position); //gets distance from player to the level indicator (this)

        if (distance <= particleRadius) // check player is within distance of the level indicator
        {            
            if (Input.GetKey(KeyCode.Space)) // counts time the space key is being held for
            {
                keyHoldTime += Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.Space) && keyHoldTime >= 1f) // triggers time to load level scene if space is currently being held for more than a few seconds, playing the start music and turning on the fade in animation
            {
                timerOn = true;
                fadeIn.SetActive(true);
                startMusic.SetActive(true);
            }   
        }

        if (timerOn == true) //timer is true when level is ready to start after previous trigger
        {
            timer += Time.deltaTime;
            if (timer >= 2.0f) //loads level scene once a few seconds have passed after holding down space within the level indicator (allowing time for the fade in animation to complete)
            {

                switch (this.tag) //checks tag of current level indicator to load corresponding level
                {
                    case "suburb":
                        levelSetting = "SuburbLevels";
                        break;
                    case "park":
                        levelSetting = "ParkLevels";
                        break;
                    case "town":
                        levelSetting = "TownLevels";
                        break;
                    case "manhole":
                        levelSetting = "ManholeLevels";
                        break;
                }

                keyHoldTime = 0.0f;
                LevelOverMenu.isLevelOver = false; //ensures game can be paused when starting a new level
                SceneManager.LoadScene(levelSetting, LoadSceneMode.Single);
            }
        }
    }



}

