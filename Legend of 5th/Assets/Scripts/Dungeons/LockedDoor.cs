using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoor : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<PlayerInventory>().keys > 0)
            {
                other.GetComponent<PlayerInventory>().keys -= 1;
                Destroy(gameObject);
            }
        }
    }
}
