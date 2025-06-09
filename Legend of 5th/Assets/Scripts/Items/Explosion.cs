using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private float timer = 5;
    // Update is called once per frame
    void Update()
    {
        timer -= 1;
        if (timer <= 0)
        {
            Destroy(gameObject);
        }
    }
}
