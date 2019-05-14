using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBehaviour : MonoBehaviour
{
    /* 
     The SpawnBehaviour script is attached to all the spawned obstacles and power-ups within the levels. It is responsible for detecting collisions between the object and either the killbox (which destroys the object), or the player or dog (which determines whether a life should be lost or added etc.).
     */

    int lives;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {    
    }

    void OnTriggerEnter(Collider collision) //OnTriggerEnter is a Unity function that holds information about collisions in the collision variable
    {
        if (collision.gameObject.name == "KillBox") //destroys game object if it collides with the killbox; ie. goes past the player (prevents objects from making a second round on the conveyor)
        {
            Destroy(gameObject);
        }

        if (collision.gameObject.name == "LevelPlayer" || collision.gameObject.name == "Dog" && LevelPauseMenu.pause == false) //Triggers following code if game object colliders with the player or the dog, and the game isn't paused
        {
            lives = LevelPlayerController.lives; //gets current lives
            switch (this.name) //gets the name of the current game object to refer to. If object is a fire hydrant, manhole, or poop, and the player has 1 or more lives, player loses a life and obstacle audio plays. If object is an energy drink or dog bone, and the player has less than 5 lives, player gains a life and power up audio plays.
            {
                case "firehydrant Variant(Clone)":
                    if (lives >= 1)
                    { 
                        lives -= 1;
                        LevelPlayerController.audioPlayObstacle = true;
                    }   
                    break;
                case "manhole Variant(Clone)":
                    if (lives >= 1)
                    {
                        lives -= 1;
                        LevelPlayerController.audioPlayObstacle = true;
                    }
                    break;
                case "poop Variant(Clone)":
                    if (lives >= 1)
                    {
                        lives -= 1;
                        LevelPlayerController.audioPlayObstacle = true;
                    }
                    break;
                case "energydrink Variant(Clone)":
                    if (lives < 5)
                    {
                        lives +=1;
                        LevelPlayerController.audioPlayPowerUp = true;
                    }
                    break;
                case "bone Variant(Clone)":
                    if (lives < 5)
                    {
                        lives += 1;
                        LevelPlayerController.audioPlayPowerUp = true;
                    }
                    break;
            }
            LevelPlayerController.lives = lives; //updates lives
            Destroy(gameObject); //game object is destroyed upon collision with the player or dog. Prevents from accidentally double-colliding with the same object
        }       
    }
}
