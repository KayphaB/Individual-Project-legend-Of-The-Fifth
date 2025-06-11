using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableDrop : MonoBehaviour
{
    public int mycopurite;
    public int keys;
    public int shrombs;
    public float health;
    private ClassicFollow cf;
    private void Start()
    {
        cf = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ClassicFollow>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerInventory>().mycopurite += mycopurite;
            other.GetComponent<PlayerInventory>().keys += keys;
            other.GetComponent<PlayerInventory>().shrombs += shrombs;
            other.transform.GetChild(0).transform.GetComponent<PlayerHealth>().HP += health;

            if (keys > 0)
            {
                cf.keyBatHere[(int) (Mathf.Floor((cf.transform.position.x - cf.offsetX) / cf.screenLengthX) +
                Mathf.Floor((cf.transform.position.y - cf.offsetY) / cf.screenLengthY) * 5 + 66)] = false;
            }
            Destroy(gameObject);
        }
    }
}
