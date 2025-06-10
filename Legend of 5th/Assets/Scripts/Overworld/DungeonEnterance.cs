using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DungeonEnterance : MonoBehaviour
{
    public Vector3 destination;
    public GameObject camera;
    private ClassicFollow camScript;

    public PlayerHealth ph;

    public ScreenFade screenFade;
    private bool duplicate;
    void Start()
    {
        camScript = camera.GetComponent<ClassicFollow>();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        //if the player enters the trigger and is facing towards the cave, then teleport the player to the set destination
        if (other.name == "Player" && !duplicate)
        {
            duplicate = true;
            StartCoroutine(CaveEnter(other));
        }
    }

    IEnumerator CaveEnter(Collider2D player)
    {
        //freeze player movement and wait until screen fades to black
        player.GetComponent<PlayerController>().enteringCave = true;
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

        //set new checkpoint to dungeon enterance
        ph.SetCheckpoint();

        //un-freeze player controls
        player.GetComponent<PlayerController>().enteringCave = false;

        duplicate = false;
    }
}