using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelOverUIText : MonoBehaviour
{
    /*
     The LevelOverUIText script is responsible for retrieving the final score of the level, to convert and display in the level over UI.
    */

    LevelPlayerController player;
    public Text scoreText;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = LevelPlayerController.distanceInt.ToString() + "m"; //gets final score from the LevelPlayerController script, adds 'm' for meters
    }
}