using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CaveEnterance : MonoBehaviour
{
    public Vector3 destination;
    public GameObject camera;
    private CameraFollow camScript;
    public GameObject caveExit;
    public string caveText;
    public TMP_Text caveTextObject;
    void Start()
    {
        camScript = camera.GetComponent<CameraFollow>();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        //if the player enters the trigger and is facing towards the cave, then teleport the player to the set destination
        if (other.name == "Player")
        {
            if (other.GetComponent<PlayerController>().direction == 3)
            {
                //move the camera and player to destination
                other.transform.position = destination;
                camera.transform.position = new Vector3(
                    Mathf.Round(destination.x / camScript.screenLengthX) * camScript.screenLengthX + camScript.offsetX, 
                    Mathf.Round(destination.y / camScript.screenLengthY) * camScript.screenLengthY + camScript.offsetY, 
                0);

                //set the cave exit's destination to the location of the cave entrance
                caveExit.GetComponent<CaveExit>().destination = new Vector3(
                    transform.position.x, 
                    transform.position.y - 0.5f, 
                -5);

                //set the caves text to the "caveText" variable
                caveTextObject.text = caveText;
            }
        }
    }
}
