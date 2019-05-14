using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    /* 
    The PlayerController script is attached to the player game object. This script is responsible for moving the player, turning on view mode, finding and storing the level difficulty value, respawning the player if they manage to get out of bounds, and spawning the player a dog friend.
    */

    Animator move, dogMove;
    float rotationSpeed;
    float runSpeed;
    float walkSpeed;
    GameObject cameraMouse;
    float camX, camY, camZ; //variables to hold camera reset rotation values
    public bool viewMode;
    public static int difficulty;
    public static bool hasDog;
    public static int dogType;
    int dogCount;
    GameObject[] levels;
    GameObject townEasy, townMedium, townHard, parkEasy, parkMedium, parkHard, suburbsEasy, suburbsMedium, suburbsHard;
    GameObject bulletinParticles, townManhole, parkManhole, suburbManhole;
    GameObject introductionUI;
    float townManholeD, parkManholeD, suburbManholeD;   
    float distance;
    float particleRadius;
    float smallParticleRadius;

    [SerializeField]
    GameObject footstep, woof, dog, dogBlack, dogBlonde, dogGrey, dogWhite;

    // Start is called before the first frame update
    void Start()
    {
        hasDog = false;
        dogType = 0;
        dogCount = 0;
        move = gameObject.GetComponentInChildren<Animator>(); //reference to player animator
        dogMove = gameObject.GetComponentInChildren<Animator>();
        cameraMouse = GameObject.Find("Camera");
        rotationSpeed = 200.0f;
        runSpeed = 4.0f;
        walkSpeed = 1.0f;
        camX = 20f;
        camY = 0.0f;
        camZ = 0.0f;
        cameraMouse.GetComponent<MouseCamera>().enabled = false;
        viewMode = false;
        distance = 0.0f;
        difficulty = 0;
        introductionUI = GameObject.Find("IntroductionUI");

        //LEVELS
        bulletinParticles = GameObject.Find("Bulletin particles");
        townEasy = GameObject.Find("TownEasy");
        townMedium = GameObject.Find("TownMedium");
        townHard = GameObject.Find("TownHard");
        parkEasy = GameObject.Find("ParkEasy");
        parkMedium = GameObject.Find("ParkMedium");
        parkHard = GameObject.Find("ParkHard");
        suburbsEasy = GameObject.Find("SuburbsEasy");
        suburbsMedium = GameObject.Find("SuburbsMedium");
        suburbsHard = GameObject.Find("SuburbsHard");
        levels = new GameObject[9]; //levels array stores references to all of the level indicators within the hub
        levels[0] = townEasy;
        levels[1] = parkEasy;
        levels[2] = suburbsEasy;
        levels[3] = townMedium;
        levels[4] = parkMedium;
        levels[5] = suburbsMedium;
        levels[6] = townHard;
        levels[7] = parkHard;
        levels[8] = suburbsHard;

        townManhole = GameObject.Find("townManhole");
        parkManhole = GameObject.Find("parkManhole");
        suburbManhole = GameObject.Find("suburbManhole");

        particleRadius = townEasy.GetComponent<ParticleSystem>().shape.radius;
        smallParticleRadius = bulletinParticles.GetComponent<ParticleSystem>().shape.radius;

        if (StartMenu.firstGame == false) //prevents introduction text from appearing if the player is returning after a level
        {
            introductionUI.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //trigger for introduction text to fade away upon first load
        if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.V) || Input.GetKey(KeyCode.P) || Input.GetKey(KeyCode.Escape)) && StartMenu.firstGame == true)
        {
            StartMenu.firstGame = false;
            introductionUI.GetComponent<Animator>().enabled = true;
        }

        //VIEW MODE (free view with mouse, disables player movement)
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (viewMode == false && PauseMenu.pause == false) //goes into view mode if not already, game isn't paused, and v key is pressed
            {
                viewMode = true;
                cameraMouse.GetComponent<MouseCamera>().enabled = true;               
            }
            else //if in view mode, and v is pressed, toggle view mode off & reset camera position
            {
                viewMode = false;
                cameraMouse.GetComponent<MouseCamera>().enabled = false;
                camY = this.transform.eulerAngles.y; //update camera's Y reset position rotation to be that of the player's current Y rotation
                cameraMouse.transform.eulerAngles = new Vector3(camX, camY, camZ); //reset camera to be facing player                
            }
        }       

        //PLAYER MOVEMENT
        if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) && viewMode == false && PauseMenu.pause == false) //moves player forward if up arrow or w is pressed, game isn't paused, and not in view mode
        {
            move.SetInteger("RunOn", 1); //turns on run animation
            footstep.SetActive(true); //footstep audio is on when moving 
            transform.Translate(Vector3.forward * runSpeed * Time.deltaTime); //forward control

            if (hasDog == true) //if player has a dog, sets dog animation to run also
            {
                dogMove.SetInteger("RunOn", 1);
            }
        }
        else //when idle
        {
            move.SetInteger("RunOn", 0); //turns off run animation
            footstep.SetActive(false); //turns off footstep audio 

            if (hasDog == true) //if player has a dog, sets dog animation to idle also
            {
                dogMove.SetInteger("RunOn", 0);
            }
        }

        if ((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) && viewMode == false) //moves player backwards if down arrow or s is pressed, game isn't paused, and not in view mode
        {
            move.SetInteger("WalkOn", 1); //turns on walk animation
            transform.Translate(Vector3.back * walkSpeed * Time.deltaTime); //backward control

            if (hasDog == true) //if player has a dog, sets dog animation to walk also
            {
                dogMove.SetInteger("WalkOn", 1);
            }
        }
        else
        {
            move.SetInteger("WalkOn", 0); //turns off run animation
            if (hasDog == true) //if player has a dog, sets dog animation to idle also
            {
                dogMove.SetInteger("WalkOn", 0);
            }
        }

        if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && viewMode == false) //rotate camera left if not in view mode, and left arrow or a is pressed
        {
            transform.Rotate(Vector3.down * rotationSpeed * Time.deltaTime);
        }
        else if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && viewMode == false) //rotate camera right if not in view mode, and right arrow or d is pressed
        {
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        }

        //KILL ZONE (out of bounds)
        if (transform.position.x <= -35 || transform.position.x >= -2 || transform.position.z <= -11 || transform.position.z >= 78)
        {
            transform.position = new Vector3(-19.59f, 0.285f, -4.885f); //teleports player back to start if they somehow get out of bounds
        }

        
        //LEVEL DIFFICULTY
        if (Input.GetKeyDown(KeyCode.Space)) { //following code runs if space is pressed

            //SETTING NORMAL LEVEL DIFFICULTIES
            for (int i = 0; i < levels.Length; i++) //for loops through the array of all the level indicators in the scene; if player is within radius of any of them, sets difficulty corresponding to index of array (first 3 indexes are easy level indicators, next 3 medium level indicators, last 3 hard level indicators)
            {
                distance = Vector3.Distance(levels[i].transform.position, this.transform.position);
                if (distance <= particleRadius) // check player is within distance of the level indicator
                {
                    if (i >= 0 && i <= 2)
                    {
                        difficulty = 1; //easy difficulty value
                    }
                    else if (i >= 3 && i <= 5)
                    {
                        difficulty = 2; //medium difficulty value
                    }
                    else if (i >= 6 && i <= 8)
                    {
                        difficulty = 3; //hardest difficulty value
                    }
                }
            }

            //SETTING MANHOLE DIFFICULTY
            townManholeD = Vector3.Distance(townManhole.transform.position, this.transform.position); //gets distance of player to all 3 manholes in the hub scene
            parkManholeD = Vector3.Distance(parkManhole.transform.position, this.transform.position);
            suburbManholeD = Vector3.Distance(suburbManhole.transform.position, this.transform.position);
            if (townManholeD <= smallParticleRadius || parkManholeD <= smallParticleRadius || suburbManholeD <= smallParticleRadius) // check player is within radius of any of the manholes; if so, sets difficulty to hardest
            {
                difficulty = 4;
            }

            //GETTING A DOG
            if (hasDog == true && dogCount == 0) //player only obtains a dog if they currently don't have one, and hasDog has been triggered on from the AIDogBehaviour script
            { 
                woof.SetActive(true); //plays bark audio when dog is obtained
                dogCount += 1; //updates dogCount variable to prevent player from getting any more dogs
                switch (dogType) //dogType variable is updated once trigger has been performed in the AIDogBehaviour script. Corresponding dog colour game object is set active as per below
                {
                    case 1:
                        dog.SetActive(true);
                        dogMove = dog.GetComponentInChildren<Animator>();
                        break;
                    case 2:
                        dogBlack.SetActive(true);
                        dogMove = dogBlack.GetComponentInChildren<Animator>();
                        break;
                    case 3:
                        dogBlonde.SetActive(true);
                        dogMove = dogBlonde.GetComponentInChildren<Animator>();
                        break;
                    case 4:
                        dogGrey.SetActive(true);
                        dogMove = dogGrey.GetComponentInChildren<Animator>();
                        break;
                    case 5:
                        dogWhite.SetActive(true);
                        dogMove = dogWhite.GetComponentInChildren<Animator>();
                        break;
                }
            }
        }

    }

}
