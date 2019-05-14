using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUIText : MonoBehaviour
{
    /* 
     The LevelUIText script is attached to the level overlay UI in the levels, and is used to convert the life and distance score values from variables to strings for visible use in the UI.
    */

    LevelPlayerController player;
    public Text livesText;
    public Text distanceText;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        livesText.text = LevelPlayerController.lives.ToString();
        distanceText.text = LevelPlayerController.distanceInt.ToString() + "m";
    }
}
