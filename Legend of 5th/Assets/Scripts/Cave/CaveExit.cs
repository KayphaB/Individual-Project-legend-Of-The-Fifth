using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveExit : MonoBehaviour
{
    public Vector3 destination;
    public GameObject camera;
    private ClassicFollow camScript;
    public ScreenFade screenFade;
    public bool leaveUp;
    private bool duplicate;
    void Start()
    {
        camScript = camera.GetComponent<ClassicFollow>();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        //if the player enters the trigger and is facing towards the exit, then teleport the player to the set cave entrance
        if (other.name == "Player" && !duplicate)
        {
            if (other.GetComponent<PlayerController>().direction == 4)
            {
                duplicate = true;
                StartCoroutine(CaveLeave(other));   
            }
        }
    }

    IEnumerator CaveLeave(Collider2D player)
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
        -10);
        
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
