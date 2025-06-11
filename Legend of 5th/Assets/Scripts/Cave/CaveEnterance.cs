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
    public UnlockablesManager unlockables;

    public bool[] hasItem;
    public GameObject[] itemPickup;
    public Sprite[] itemVisuals;
    public int[] itemLevel;
    public int[] itemID;
    public int[] cost;
    public bool[] oneTimeBuy;
    public bool[] isHeartContainer;

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
            itemPickup[1].SetActive(hasItem[1]);

            if (hasItem[1])
            {
                ItemPickup pickup = itemPickup[1].GetComponent<ItemPickup>();
                pickup.cave = gameObject.GetComponent<CaveEnterance>();
                pickup.isHeartContainer = isHeartContainer[1];
                if (itemLevel[1] == 0)
                {
                    pickup.unlocks = true;
                    pickup.givesItem = false;
                    pickup.unlock = itemID[1];
                    pickup.ID = 1;
                    itemPickup[1].GetComponent<SpriteRenderer>().sprite = itemVisuals[1];
                }
                else
                {
                    pickup.unlocks = false;
                    pickup.givesItem = true;
                    pickup.item = itemID[1];
                    pickup.ID = 1;
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
                pickup.gameObject.SetActive(hasItem[i]);
                if (hasItem[i])
                {
                    pickup.cave = gameObject.GetComponent<CaveEnterance>();
                    pickup.isHeartContainer = isHeartContainer[i];
                    if (itemLevel[i] == 0)
                    {
                        pickup.unlocks = true;
                        pickup.givesItem = false;
                        pickup.cost = cost[i];
                        pickup.unlock = itemID[i];
                        pickup.ID = i;
                        itemPickup[i].GetComponent<SpriteRenderer>().sprite = itemVisuals[i];
                    }
                    else
                    {
                        pickup.unlocks = false;
                        pickup.givesItem = true;
                        pickup.cost = cost[i];
                        pickup.item = itemID[i];
                        pickup.itemLevel = itemLevel[i];
                        pickup.ID = i;
                        itemPickup[i].GetComponent<SpriteRenderer>().sprite = itemVisuals[i];
                    }
                }
            }
        }
        else if (destination.y == -17)
        {
            Debug.Log(unlockables.unlockables[0] && unlockables.unlockables[2]);
            itemPickup[1].SetActive(unlockables.unlockables[0] && unlockables.unlockables[2]);

            ItemPickup pickup = itemPickup[1].GetComponent<ItemPickup>();
            pickup.cave = gameObject.GetComponent<CaveEnterance>();
            pickup.isHeartContainer = isHeartContainer[1];
            pickup.unlocks = true;
            pickup.givesItem = false;
            pickup.cost = 0;
            pickup.unlock = itemID[1];
            pickup.ID = 1;
            itemPickup[1].GetComponent<SpriteRenderer>().sprite = itemVisuals[1];
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
