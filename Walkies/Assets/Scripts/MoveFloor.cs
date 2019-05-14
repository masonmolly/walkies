using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFloor : MonoBehaviour {

    /* 
    The MoveFloor script is attached to all of the road game objects found in the level scenes. This script is responsible for moving the roads in a conveyor-like fashion, and randomly generating the obstacles and power-ups.
    */

    int difficulty;
    GameObject player;
    float speed;
    GameObject road;
    float timer = 0f;
    GameObject spawn;
    Vector3 spawnArea;
    int spawnX;
    float spawnZ;
    bool pause;
    int lives;
    float randLower;
    float randHigher;

    [SerializeField]   //serialized fields to hold the obstacle and power up game object prefabs
    GameObject hydrant, poop, manhole, treat, drink;
    
	// Use this for initialization
	void Start () {
        player = GameObject.Find("LevelPlayer");
        difficulty = 1; //variable initialisation
        difficulty = PlayerController.difficulty; //gets current level difficulty from PlayerController script
        lives = 3;

        switch (this.name) //switch statement decides which road the current road needs to spawn to the back of when it teleports back; like a conveyor chain. road 1 -> road 3 -> road 2 -> road 1. 
        {
            case "Road1":
                GameObject road3 = GameObject.Find("Road3");
                road = road3;
                break;
            case "Road2":
                GameObject road1 = GameObject.Find("Road1");
                road = road1;
                break;
            case "Road3":
                GameObject road2 = GameObject.Find("Road2");
                road = road2;
                break;
        }

        switch (difficulty) //changes road speed, and rate of spawns dependent on level difficulty; higher the difficulty the higher the road speed and more often the spawns
        {
            case 1:
                speed = 0.3f;
                randLower = 2f;
                randHigher = 3.5f;
                break;
            case 2:
                speed = 0.4f;
                randLower = 1.5f;
                randHigher = 3f;
                break;
            case 3:
                speed = 0.5f;
                randLower = 1f;
                randHigher = 2.5f;
                break;
            case 4:
                speed = 0.6f;
                randLower = 1f;
                randHigher = 2f;
                break;
        }
    }
	
	// Update is called once per frame
	void Update () {
        lives = LevelPlayerController.lives; //gets updated lives from LevelPlayerController script
        pause = LevelPauseMenu.pause; //gets pause state of level from LevelPauseMenu script
        Vector3 pos = transform.position; //variable pos holds current position of road
        float roadSize = this.GetComponent<Renderer>().bounds.size.z;

        //ROAD MOVEMENT
        if (pause == false && lives > 0) //road will continue to move as long as the game isn't paused or the player is out of lives
        {
            this.transform.Translate(0, 0, -speed);
        }
        if (this.transform.position.z <= (player.transform.position.z - roadSize/2 - 5)) //checks road position in the world, if completely past the player the road will teleport to the back of the last road in the 'conveyor'. the road variable refers to the road ahead of this road in the conveyor. Z position refers to the center of the road object, and so this checks for the z position minus half of the road length in comparison to the player position.
        {
            pos.z = road.transform.position.z + roadSize;
            transform.position = pos;
        }

        //OBJECT SPAWNING
        timer += Time.deltaTime;
        float rand = Random.Range(randLower, randHigher); //generates random value based on level difficulty to determine time range between new spawns
        if (rand <= timer && pause == false && this.transform.position.z >= 50) //if game isn't paused, and position is sufficiently far from the player, game will spawn a new object every x seconds
        {
            timer = 0f;
            spawnGen();
        }

	}

    void spawnGen() //spawnGen function randomly spawns an obstacle or power-up.
    {
        float spawnGen = Random.Range(0f, 100f); //generates random value to choose a spawn object

        if (spawnGen >= 0f && spawnGen <= 26f) //25% chance for spawn to be a fire hydrant
        {
            spawn = hydrant;
        }
        else if (spawnGen >= 26f && spawnGen <= 51f) //25% chance for spawn to be a poop
        {
            spawn = poop;
        }
        else if (spawnGen >= 51f && spawnGen <= 81f) //30% chance for spawn to be a manhole
        {
            spawn = manhole;
        }
        else if (spawnGen >= 81f && spawnGen <= 91f) //10% chance for spawn to be a dog bone
        {
            spawn = treat;
        }
        else if (spawnGen >= 91f && spawnGen <= 101f) //10% chance for spawn to be an energy drink
        {
            spawn = drink;
        }
        else //Spawns fire hydrant and debug text if somehow none of the previous spawn
        {
            spawn = hydrant;
            print(spawnGen);
            print("You shouldn't be here!");
        }

        spawnX = Random.Range(16, 25); //generates random x coordinate within the road
        float zLower = this.transform.position.z - 25;
        float zUpper = this.transform.position.z + 25;
        spawnZ = Random.Range(zLower, zUpper); //generates random z coordinate within the current road

        var obstacle = Instantiate(spawn, new Vector3(spawnX, 0.5f, spawnZ), Quaternion.identity); //instantiates new spawn based on the previously determined variables
        obstacle.transform.parent = this.transform; //sets the just-spawned object's parent to be this road; thus the object moves along on the road  
    }
}
