using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIBehaviour : MonoBehaviour
{
    /*
     The AIBehaviour script is attached to all of the human AIs in the hub scene, and is responsible for all of their behaviour; idling, walking, running, and flocking.
    */

    GameObject[] humans;
    Animator move;
    int collisionAmount = 0;
    NavMeshAgent nav;
    float action;
    float speed;
    float timer, timer1, rand;
    int respawnCount;
    Vector3 destination; //destination variable holds the AI's current destination

    // Start is called before the first frame update
    void Start()
    {
        humans = GameObject.FindGameObjectsWithTag("human"); //finds all of the human AI currently in the scene
        move = gameObject.GetComponentInChildren<Animator>();
        nav = GetComponent<NavMeshAgent>(); //find the NavMeshAgent component to refer to

        respawnCount = 0; //AI starts off with no respawns, variable is used to ensure they can only respawn once
    }

    // Update is called once per frame
    void Update()
    {
        speed = nav.velocity.magnitude; //speed variable gets the AI's current speed

        if (gameObject.transform.position.y > 0.8f && respawnCount == 0) //if the AI has spawned on top of something (i.e. where it shouldn't be), teleport it to new coordinates by calling the respawn function
        {
            respawn();
        }
       
        if (speed > 0.0f && speed <= 1.7f) //if the speed is more than 0, but not too fast (less than 1.7), set AI animation to walk
        {
            move.SetInteger("WalkOn", 1);
        }
        else if (speed >= 1.7f) //if the speed is more than 1.7, set AI animation to run
        {
            move.SetInteger("WalkOn", 0);
            move.SetInteger("RunOn", 1);
        }
        else //if the speed is none of the above (thus 0 or less), set AI animation to idle
        {
            move.SetInteger("WalkOn", 0);
            move.SetInteger("RunOn", 0);
        }

        if (Vector3.Distance(destination, this.transform.position) < 3f) //if the AI is very near its destination for too long (indicating it's possibly stuck), its path gets reset and it rolls a new action
        {
            timer1 += Time.deltaTime;
            if (timer1 > 15f)
            {
                nav.ResetPath();
                chooseAction();
                timer1 = 0f;
            }
        }

        timer += Time.deltaTime;
        rand = Random.Range(3f, 25f);
        if (rand <= timer && speed == 0.0f) //every few seconds, if the AI isn't moving, roll a new action for the AI to perform
        {
            timer = 0f;
            chooseAction();
        }        
    }

    void respawn() //respawn function moves the AI to a new random position within the hub
    {
        respawnCount += 1;
        float spawnX = Random.Range(-28f, -10f);
        float spawnZ = Random.Range(-6f, 72f);
        this.transform.position = new Vector3(spawnX, 0.3f, spawnZ);
    }

    void chooseAction() //chooseAction function calls one of the 4 action functions; weighted differently so not completely random.
    {
        action = Random.Range(0f, 100f); //generates random number to call corresponding action
        if (action >= 0f && action <= 60f) //60% chance of idling
        {
            idle();
        }
        else if (action >= 60f && action <= 75f) //15% chance of walking 
        {
            walk();
        }
        else if (action >= 75f && action <= 80f) //5% chance of running
        {
            run();
        }
        else if (action >= 80f) //20% chance of flocking
        {
            flock();
        }
        else
        {
            idle();
        }
    }

    void idle() //empty function to do nothing, idle the AI
    {
    }

    void walk() //walk function chooses a random coordination within the hub, sets the AI destination to said coordinate - AI walks
    {
        nav.speed = 1; //sets AI speed to walking speed
        Vector3 walkDestination;
        float destX = Random.Range(-28f, -10f);
        float destZ = Random.Range(-6f, 72f);
        walkDestination = new Vector3(destX, 0.3f, destZ);
        destination = walkDestination;
        nav.SetDestination(walkDestination); //SetDestination function is a built in Unity function that uses NavMesh pathfinding to move the AI agent to the specified destination
    }

    void run() //run function chooses a random coordination within the hub, sets the AI destination to said coordinate - AI runs
    {
        nav.speed = 2.5f; //sets AI speed to running speed
        Vector3 runDestination;
        float destX = Random.Range(-28f, -10f);
        float destZ = Random.Range(-6f, 72f);
        runDestination = new Vector3(destX, 0.3f, destZ);
        destination = runDestination;
        nav.SetDestination(runDestination);
    }

    void flock() //flock function directs the AI to the nearest human, encouraging flocking behaviour
    {
        nav.speed = 1f; //sets AI speed to walking speed
        float distance;
        float[] distances;
        float[] minMax;
        float lowest;
        int higher = 2;
        GameObject target;
        Vector3 flockDestination = this.transform.position; //initialisation value
        distances = new float[humans.Length];
        minMax = new float[humans.Length];        

        for (int i = 0; i < humans.Length; i++) //for loop to make array of distances from current human to all other humans
        {
            distance = Vector3.Distance(humans[i].transform.position, this.transform.position);
            distances[i] = distance;          
        }

        minMax = distances; //duplicate array for storage of sorted values

        for (int k = 0; k < minMax.Length-1; k++) //bubble sort through distances array, sorting from lowest to highest and storing in separate array (minMax)
        {
            for (int l = 0; l < minMax.Length-1; l++)
            {
                if (minMax[l] > minMax[l+1])
                {
                    float temp = minMax[l+1];
                    minMax[l+1] = minMax[l];
                    minMax[l] = temp;
                }
            }
        }

        lowest = minMax[1]; //array includes the person this script is applied to; thus the nearest person is the second value in the array (minMax[0] being 0)

        while (lowest < 5f && higher > 10) //if the nearest person is too near (within 10f distance), carry on increasing chosen target until they're not within 10f
        {
            lowest = minMax[higher];
            higher += 1;
        }

        if (lowest < 5f) //if there are no people further than 10f away, break out of function (and thus don't flock)
        {
            return;
        }

        for (int i = 0; i < distances.Length; i++) //for loop finds the human the lowest distance value corresponds to (as determined above); sets destination to their position
        {
           if (lowest == distances[i])
           {
                target = humans[i];
                flockDestination = humans[i].transform.position; 
           }
        }

        destination = flockDestination;
        nav.SetDestination(flockDestination); //sets human's destination to their target
    }

    //The following function is commented out as it was used when I was testing around with the idea of the AI doing something when the player collided with them, such as saying increasingly offended comments dependent on how much the player had collided with them.

    /*void OnCollisionStay(Collision collider)
    {
        if (collider.gameObject.name == "Player")
        {
            collisionAmount += 1;
            if (collisionAmount >= 1 && collisionAmount <= 10)
            {
                print("Nice to meet you!");
            }
            else if (collisionAmount >= 10 && collisionAmount <= 20)
            {
                print("Hello!");
            }
            else if (collisionAmount >= 20 && collisionAmount <= 30)
            {
                print("How rude.");
            }
            else if (collisionAmount >= 30 && collisionAmount <= 50)
            {
                print("Stop pushing me.");
            }
            else if (collisionAmount >= 50)
            {
                print("Go away!");
            }
        }
    }
    */
}
