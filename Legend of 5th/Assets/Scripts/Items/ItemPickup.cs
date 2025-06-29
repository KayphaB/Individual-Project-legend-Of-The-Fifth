using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemPickup : MonoBehaviour
{
    public InventoryManager im;
    public bool givesItem;
    public int item;
    public int itemLevel;
    public bool isHeartContainer;

    public UnlockablesManager um;
    public bool unlocks;
    public int unlock;

    public CaveEnterance cave;
    public int ID;

    public int cost;
    public TextMeshPro costText;
    void Update()
    {
        if (cost != 0)
        {
            costText.text = cost.ToString();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.GetComponent<PlayerInventory>().mycopurite >= cost)
        {
            if (isHeartContainer)
            {
                other.transform.GetChild(0).GetComponent<PlayerHealth>().maxHP += 1;
                other.transform.GetChild(0).GetComponent<PlayerHealth>().HP = 100;
            }
            else
            {
                if (givesItem)
                {
                    im.itemsUnlocked[item] = itemLevel;
                }

                if (unlocks)
                {
                    um.unlockables[unlock] = true;
                }
            }

            if (cave != null && cave.oneTimeBuy[ID])
            {
                cave.hasItem[ID] = false;
            }

            other.GetComponent<PlayerInventory>().mycopurite -= cost;

            gameObject.SetActive(false);
        }
    }
}
