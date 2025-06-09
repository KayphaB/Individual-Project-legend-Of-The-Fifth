using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableDrop : MonoBehaviour
{
    public int mycopurite;
    public int keys;
    public int shrombs;
    public float health;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerInventory>().mycopurite += mycopurite;
            other.GetComponent<PlayerInventory>().keys += keys;
            other.GetComponent<PlayerInventory>().shrombs += shrombs;
            other.transform.GetChild(0).transform.GetComponent<PlayerHealth>().HP += health;
            Destroy(gameObject);
        }
    }
}
