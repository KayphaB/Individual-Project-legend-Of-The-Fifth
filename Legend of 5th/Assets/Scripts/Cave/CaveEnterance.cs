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
    public ScreenFade screenFade;
    private bool duplicate;
    void Start()
    {
        camScript = camera.GetComponent<CameraFollow>();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        //if the player enters the trigger and is facing towards the cave, then teleport the player to the set destination
        if (other.name == "Player" && !duplicate)
        {
            if (other.GetComponent<PlayerController>().direction == 3)
            {
                duplicate = true;
                StartCoroutine(CaveEnter(other));   
            }
        }
    }

    IEnumerator CaveEnter(Collider2D player)
    {
        //freeze player movement and wait until screen fades to black
        player.GetComponent<PlayerController>().frozen = true;
        screenFade.timer = 0;
        screenFade.on = true;
        yield return new WaitForSeconds(screenFade.delay / 50 * 4f);

        //move the camera and player to destination
        player.transform.position = destination;
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
        
        //wait a bit, then start un-fading the screen
        yield return new WaitForSeconds(screenFade.delay / 50);
        screenFade.timer = 0;
        screenFade.on = false;
        yield return new WaitForSeconds(screenFade.delay / 50 * 3f);

        //un-freeze player controls
        player.GetComponent<PlayerController>().frozen = false;

        duplicate = false;
    }
}
