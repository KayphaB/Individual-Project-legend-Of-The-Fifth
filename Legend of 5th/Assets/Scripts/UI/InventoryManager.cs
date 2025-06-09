using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public GameObject[] itemVisuals;
    public int[] itemsUnlocked;
    public int selected;
    public int hovering;
    public GameObject selector;
    public GameObject hoverer;
    public GameObject itemUsedVisual;
    public GameObject itemUsedJ;

    public Sprite[] level1Visuals;
    public Sprite[] level2Visuals;

    private PlayerController pc;
    private void Start()
    {
        pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
    void Update()
    {
        //place selectors and hoveror where appropriate
        hoverer.transform.position = new Vector3(
            hovering * 80 + 440, 
            hoverer.transform.position.y, 
            hoverer.transform.position.z);

        selector.transform.position = new Vector3(
            selected * 80 + 440,
            selector.transform.position.y,
            selector.transform.position.z);

        //show and hide the appropriate items
        for (int i = 0;i < itemVisuals.Length;i++)
        {
            if (itemsUnlocked[i] > 0)
            {
                itemVisuals[i].SetActive(true);
            }
            else
            {
                itemVisuals[i].SetActive(false);
            }
        }

        //set items to the appropriate sprite
        for (int i = 0; i < itemVisuals.Length; i++)
        {
            if (itemsUnlocked[i] == 2)
            {
                itemVisuals[i].GetComponent<Image>().sprite = level2Visuals[i];
            }
            else
            {
                itemVisuals[i].GetComponent<Image>().sprite = level1Visuals[i];
            }
        }

        //set the item currently beign used to the currently selected item
        itemUsedVisual.GetComponent<Image>().sprite = itemVisuals[selected].GetComponent<Image>().sprite;
        itemUsedJ.GetComponent<Image>().sprite = itemVisuals[selected].GetComponent<Image>().sprite;

        //detect when A and D keys pressed to move hovering
        if (Input.GetKeyDown(KeyCode.A) && pc.openInventory)
        {
            hovering -= 1;
            if (hovering < 0)
            {
                hovering = 4;
            }

            while (itemsUnlocked[hovering] == 0)
            {
                hovering -= 1;
                if (hovering < 0)
                {
                    hovering = 4;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.D) && pc.openInventory)
        {
            hovering += 1;
            if (hovering > 4)
            {
                hovering = 0;
            }

            while (itemsUnlocked[hovering] == 0)
            {
                hovering += 1;
                if (hovering > 4)
                {
                    hovering = 0;
                }
            }
        }

        //detect when J key is pressed to make the hovering item selected
        if (Input.GetKeyDown(KeyCode.J) && pc.openInventory)
        {
            selected = hovering;
        }
    }
}
