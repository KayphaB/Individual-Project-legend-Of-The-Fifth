using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    void Start()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
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

            gameObject.SetActive(false);
        }
    }
}
