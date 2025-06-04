using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCheckers : MonoBehaviour
{
    public bool isColliding;
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Environment"))
        {
            isColliding = true;
        }
        else
        {
            isColliding = false;
        }
    }
}
