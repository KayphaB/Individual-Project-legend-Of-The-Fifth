using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CaveEnterance : MonoBehaviour
{
    public Vector3 destination;
    public GameObject camera;
    private ClassicFollow camScript;
    public GameObject caveExit;
    public bool leaveUp;
    public string caveText;

    public bool hasItem;
    public GameObject[] itemPickup;
    public Sprite[] itemVisuals;
    public int[] itemLevel;
    public int[] itemID;
    public int[] cost;

    public TMP_Text caveTextObject;
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

        //set the cave exit's destination to the location of the cave entrance
        if (leaveUp)
        {
            caveExit.GetComponent<CaveExit>().destination = new Vector3(
                transform.position.x,
                transform.position.y + 1,
                -1);
        }
        else
        {
            caveExit.GetComponent<CaveExit>().destination = new Vector3(
                transform.position.x,
                transform.position.y - 0.75f,
                -1);
        }

        //set the caves text to the "caveText" variable
        caveTextObject.text = caveText;

        //set the item pickups to the correct item
        if (destination.y == 7)
        {
            itemPickup[1].SetActive(hasItem);

            if (hasItem)
            {
                ItemPickup pickup = itemPickup[1].GetComponent<ItemPickup>();
                pickup.cave = gameObject.GetComponent<CaveEnterance>();
                if (itemLevel[1] == 0)
                {
                    pickup.unlocks = true;
                    pickup.givesItem = false;
                    pickup.unlock = itemID[1];
                    itemPickup[1].GetComponent<SpriteRenderer>().sprite = itemVisuals[1];
                }
                else
                {
                    pickup.unlocks = false;
                    pickup.givesItem = true;
                    pickup.item = itemID[1];
                    pickup.itemLevel = itemLevel[1];
                    itemPickup[1].GetComponent<SpriteRenderer>().sprite = itemVisuals[1];
                }
            }
        }
        else if (destination.y == -5)
        {
            for (int i = 0; i < 3; i++)
            {
                ItemPickup pickup = itemPickup[i].GetComponent<ItemPickup>();
                pickup.cave = gameObject.GetComponent<CaveEnterance>();
                if (itemLevel[i] == 0)
                {
                    pickup.unlocks = true;
                    pickup.givesItem = false;
                    pickup.cost = cost[i];
                    pickup.unlock = itemID[i];
                    itemPickup[1].GetComponent<SpriteRenderer>().sprite = itemVisuals[i];
                }
                else
                {
                    pickup.unlocks = false;
                    pickup.givesItem = true;
                    pickup.cost = cost[i];
                    pickup.item = itemID[i];
                    pickup.itemLevel = itemLevel[i];
                    itemPickup[1].GetComponent<SpriteRenderer>().sprite = itemVisuals[i];
                }
            }
        }

            //wait a bit, then start un-fading the screen
            yield return new WaitForSeconds(screenFade.delay / 50);
        screenFade.timer = 0;
        screenFade.on = false;
        yield return new WaitForSeconds(screenFade.delay / 50 * 3f);

        //un-freeze player controls
        player.GetComponent<PlayerController>().enteringCave = false;

        duplicate = false;
    }
}
