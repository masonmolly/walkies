using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletinUI : MonoBehaviour
{
    /*
     The BulletinUI script is attached to the bulletin UI in the hub scene. It is responsible for determining when to load the bulletin UI, dependent on whether the player is within radius of the bulletin load particles, and holds down space or not. It is also responsible for loading/unloading specific UIs within the bulletin UI; button controls.
    */

    public static bool isBulletinOn = false; //public static variable holds state of bulletin for reference

    [SerializeField]
    GameObject bulletinUI, introductionUI, controlsUI; //serialized variables to hold the various UI's used for the bulletin UI

    PlayerController viewMode;
    GameObject player;
    GameObject particles;
    float radius;
    float keyHoldTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        viewMode = GameObject.Find("Player").GetComponent<PlayerController>(); //view mode reference to only allow the bulletin to load if not in view mode
        player = GameObject.Find("Player");
        particles = GameObject.Find("Bulletin particles");
        radius = particles.GetComponent<ParticleSystem>().shape.radius; //particle radius stored for use to know when the bulletin should be loaded (when space is held down in the particles in front of the bulletin)
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(particles.transform.position, player.transform.position);  //variable constantly updating to check distance betwen the bulletin load particles and the player  
        
        if (distance <= radius) //if the player is within the radius of the bulletin load particle, player is not in view mode, game is not paused, and space is held down for a few seconds, load the bulletin AI and pause time
        {
            if (Input.GetKey(KeyCode.Space)) //counts time the space key is being held for
            {
                keyHoldTime += Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.Space) && keyHoldTime >= 1f && PauseMenu.pause == false && viewMode.viewMode == false)
            {
                isBulletinOn = true;
                keyHoldTime = 0.0f;
                bulletinUI.SetActive(true);
                Time.timeScale = 0.0f;
            }
        }
    }

    public void introductionButton() //function applied to introduction button; unloads the home UI and loads the introduction UI
    {
        introductionUI.SetActive(true);
        bulletinUI.SetActive(false);
    }

    public void controlsButton() //function applied to controls button; unloads the home UI and loads the controls UI
    {
        controlsUI.SetActive(true);
        bulletinUI.SetActive(false);
    }

    public void homeButton() //function applied to home button; unloads the control & introduction UI and loads the home UI
    {
        introductionUI.SetActive(false);
        controlsUI.SetActive(false);
        bulletinUI.SetActive(true);
    }

    public void exitButton() //function applied to exit button; closes the bulletin UI and un-pauses time
    {
        isBulletinOn = false;
        bulletinUI.SetActive(false);
        Time.timeScale = 1.0f;
    }
}

