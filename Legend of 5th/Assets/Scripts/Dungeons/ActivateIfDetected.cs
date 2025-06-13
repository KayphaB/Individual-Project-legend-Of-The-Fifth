using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateIfDetected : MonoBehaviour
{
    public string name;
    public GameObject[] gameObjects;
    void Update()
    {
        bool namePresent = GameObject.Find(name) != null;
        for (int i = 0; i < gameObjects.Length; i++)
        {
            gameObjects[i].SetActive(namePresent);
        }
    }
}
