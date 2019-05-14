using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//SCRIPT CREDIT TO https://www.youtube.com/watch?v=lYIRm4QEqro
public class MouseCamera : MonoBehaviour
{
    /*
     The MouseCamera script is responsible for the mouse controlling the camera view when in view mode. I did not write any of the code in this script myself, the credit is commented at the top.
    */

    float horizontalSpeed;
    float verticalSpeed;
    float y;
    float x;

    // Start is called before the first frame update
    void Start()
    {
        horizontalSpeed = 2.0f;
        verticalSpeed = 2.0f;
    }

    // Update is called once per frame
    void Update()
    {
        y += horizontalSpeed * Input.GetAxis("Mouse X");
        x -= verticalSpeed * Input.GetAxis("Mouse Y");
        transform.eulerAngles = new Vector3(x, y, 0.0f);
    }
}
