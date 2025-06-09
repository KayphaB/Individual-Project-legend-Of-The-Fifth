using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShromableRock : MonoBehaviour
{
    public string tagToDestroy;

    // when shromb used nearby, delete thyself
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag(tagToDestroy))
        {
            Destroy(gameObject);
        }
    }
}
