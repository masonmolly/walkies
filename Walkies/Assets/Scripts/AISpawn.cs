using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISpawn : MonoBehaviour
{
    /*
     The AISpawn script is attached to the terrain game object in the hub scene, and is responsible for spawning all of the AI randomly across the town; both dogs and humans.
    */

    [SerializeField]
    GameObject male1, male2, male3, female1, female2, female3, dogBrown, dogBlack, dogBlonde, dogGrey, dogWhite; //serialized variables to hold the AI prefabs

    GameObject[] dogs;
    GameObject[] humans;

    int humanI, dogI, spawnCount;
    float spawnX, spawnZ, spawnQ;

    // Start is called before the first frame update
    void Start()
    {
        dogs = new GameObject[5]; //dogs array holds all of the dog AI game object prefabs
        dogs[0] = dogBrown;
        dogs[1] = dogBlack;
        dogs[2] = dogBlonde;
        dogs[3] = dogGrey;
        dogs[4] = dogWhite;

        humans = new GameObject[6]; //humans array holds all of the human (male & female) AI game object prefabs
        humans[0] = male1;
        humans[1] = male2;
        humans[2] = male3;
        humans[3] = female1;
        humans[4] = female2;
        humans[5] = female3;

        spawnCount = Random.Range(15, 25); //generates random number to determine how many dogs and humans to spawn

        for (int i = 0; i < spawnCount; i++)
        {                      
            spawnX = Random.Range(-28f, -10f); //generates random x coordinate for current AI to spawn at
            spawnZ = Random.Range(-6f, 72f); //generates random z coordinate for current AI to spawn at
            spawnQ = Random.Range(0f, 360f); //generates random number for the current AI's angle to spawn as
            humanI = Random.Range(0, 6); //generates random number to pick a random AI prefab from the humans array
            Instantiate(humans[humanI], new Vector3(spawnX, 0.3f, spawnZ), Quaternion.Euler(0f, spawnQ, 0f)); //spawns a random AI at a random point within the hub with the previously determined variables

            spawnX = Random.Range(-28f, -10f); //following code is duplicated from the human spawns, except using the array of dog prefabs. new numbers get generated so the dogs don't spawn in the same coordinates as the humans
            spawnZ = Random.Range(-6f, 72f);
            spawnQ = Random.Range(0f, 360f);
            dogI = Random.Range(0, 5);
            Instantiate(dogs[dogI], new Vector3(spawnX, 0.25f, spawnZ), Quaternion.Euler(0f, spawnQ, 0f));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
