using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveExit : MonoBehaviour
{
    public Vector3 destination;
    public GameObject camera;
    private CameraFollow camScript;
    void Start()
    {
        camScript = camera.GetComponent<CameraFollow>();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        //if the player enters the trigger and is facing towards the exit, then teleport the player to the set caveEnterance
        if (other.name == "Player")
        {
            if (other.GetComponent<PlayerController>().direction == 4)
            {
                other.transform.position = destination;
                camera.transform.position = new Vector3(
                    Mathf.Round(destination.x / camScript.screenLengthX) * camScript.screenLengthX + camScript.offsetX, 
                    Mathf.Round(destination.y / camScript.screenLengthY) * camScript.screenLengthY + camScript.offsetY, 
                0);
            }
        }
    }
}
