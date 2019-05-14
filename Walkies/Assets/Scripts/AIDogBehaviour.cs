using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIDogBehaviour : MonoBehaviour
{
    /*
    The majority of this script is duplicated from AIBehaviour (with the exception of the OnCollisionStay function found at the end of the script); and thus the comments can be found on the AIBehaviour script instead of here

    The AIDogBehaviour script is attached to all of the dog AIs in the hub scene, and is responsible for all of their behaviour; idling, walking, running, flocking, and informing the PlayerController script when to get a dog accompaniment.
    */

    GameObject[] dogs;
    Animator move;
    int collisionAmount = 0;
    NavMeshAgent nav;
    float action;
    float speed;
    float timer, timer1, rand;
    GameObject player;
    int respawnCount;
    Vector3 destination;

    // Start is called before the first frame update
    void Start()
    {
        dogs = GameObject.FindGameObjectsWithTag("dog");
        move = gameObject.GetComponentInChildren<Animator>();
        nav = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player");

        respawnCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        speed = nav.velocity.magnitude;

        if (gameObject.transform.position.y > 0.8f && respawnCount == 0)
        {
            respawn();
        }
       
        if (speed > 0.1f && speed <= 1.7f)
        {
            move.SetInteger("WalkOn", 1);
        }
        else if (speed >= 1.7f)
        {
            move.SetInteger("WalkOn", 0);
            move.SetInteger("RunOn", 1);
        }
        else
        {
            move.SetInteger("WalkOn", 0);
            move.SetInteger("RunOn", 0);
        }

        if (Vector3.Distance(destination, this.transform.position) < 3f)
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
        if (rand <= timer && speed == 0.0f)
        {
            timer = 0f;
            chooseAction();
        }
    }

    void respawn()
    {
        respawnCount += 1;
        float spawnX = Random.Range(-28f, -10f);
        float spawnZ = Random.Range(-6f, 72f);
        this.transform.position = new Vector3(spawnX, 0.25f, spawnZ);
    }

    void chooseAction()
    {
        action = Random.Range(0f, 100f);
        if (action >= 0f && action <= 50f)
        {
            idle();
        }
        else if (action >= 50f && action <= 65f)
        {
            walk();
        }
        else if (action >= 65f && action <= 70f)
        {
            run();
        }
        else if (action >= 70f) //dogs have a higher chance of flocking than humans (30%)
        {
            flock();
        }
        else
        {
            idle();
        }
    }

    void idle()
    {
    }

    void walk()
    {
        nav.speed = 1;
        Vector3 walkDestination;
        float destX = Random.Range(-28f, -10f);
        float destZ = Random.Range(-6f, 72f);
        walkDestination = new Vector3(destX, 0.3f, destZ);
        destination = walkDestination;
        nav.SetDestination(walkDestination);
    }

    void run()
    {
        nav.speed = 2.5f;
        Vector3 runDestination;
        float destX = Random.Range(-28f, -10f);
        float destZ = Random.Range(-6f, 72f);
        runDestination = new Vector3(destX, 0.3f, destZ);
        destination = runDestination;
        nav.SetDestination(runDestination);
    }

    void flock()
    {
        nav.speed = 1;
        float distance;
        float[] distances;
        float[] minMax;
        float lowest;
        int higher = 2;
        GameObject target;
        Vector3 flockDestination = this.transform.position;
        distances = new float[dogs.Length];
        minMax = new float[dogs.Length];

        for (int i = 0; i < dogs.Length; i++) 
        {
            distance = Vector3.Distance(dogs[i].transform.position, this.transform.position);
            distances[i] = distance;
        }

        minMax = distances; 

        for (int k = 0; k < minMax.Length - 1; k++) 
        {
            for (int l = 0; l < minMax.Length - 1; l++)
            {
                if (minMax[l] > minMax[l + 1])
                {
                    float temp = minMax[l + 1];
                    minMax[l + 1] = minMax[l];
                    minMax[l] = temp;
                }
            }
        }

        lowest = minMax[1];

        while (lowest < 5f && higher > 10)
        {
            lowest = minMax[higher];
            higher += 1;
        }

        if (lowest < 5f) 
        {
            return;
        }

        for (int i = 0; i < distances.Length; i++)
        {
            if (lowest == distances[i])
            {
                target = dogs[i];
                flockDestination = dogs[i].transform.position;
            }
        }

        destination = flockDestination;
        nav.SetDestination(flockDestination); 
    }

    void OnCollisionStay(Collision collider) //OnCollisionStay is a Unity function that takes the information of any collision and stores it in the collider variable
    {
        if (collider.gameObject.name == "Player") //if the AI is colliding with the player, and space is held down, send information on the dog type to the PlayerController script to activate a twin dog game object to accompany the player (e.g. if space is held down by a grey dog, the player will be accompanied by a grey dog game object)
        {
            if (Input.GetKeyDown(KeyCode.Space) && PlayerController.hasDog == false)
            {
                PlayerController.hasDog = true; //bool variable prevents the player from having multiple dog accompaniments 
                switch (this.name)
                {
                    case "Dog(Clone)":
                        PlayerController.dogType = 1;
                        break;
                    case "Dog (black)(Clone)":
                        PlayerController.dogType = 2;
                        break;
                    case "Dog (blonde)(Clone)":
                        PlayerController.dogType = 3;
                        break;
                    case "Dog (grey)(Clone)":
                        PlayerController.dogType = 4;
                        break;
                    case "Dog (white)(Clone)":
                        PlayerController.dogType = 5;
                        break;
                }
            }
        }
    }
}

