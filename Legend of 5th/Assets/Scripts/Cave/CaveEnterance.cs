using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CaveEnterance : MonoBehaviour
{
    private GameObject Player;
    public Vector3 destination;
    public 
    void Start()
    {

    }

    void OnTriggerStay(Collider other)
    {
        if (other.name == "Player" && other.GetComponent<PlayerController>().direction == 3)
        {
            other.transform.position = destination;
        }
    }
}
