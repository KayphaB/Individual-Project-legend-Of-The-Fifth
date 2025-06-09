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

    public UnlockablesManager um;
    public bool unlocks;
    public int unlock;

    public CaveEnterance cave;

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
            if (givesItem)
            {
                im.itemsUnlocked[item] = itemLevel;
            }

            if (unlocks)
            {
                um.unlockables[unlock] = true;
            }

            if (cave != null)
            {
                cave.hasItem = false;
            }

            other.GetComponent<PlayerInventory>().mycopurite -= cost;

            gameObject.SetActive(false);
        }
    }
}
