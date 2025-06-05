using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHP = 3;
    public float HP = 3;
    
    private float immuneFrames;

    void Update()
    {
        //cap health at the max
        if (HP > maxHP)
        {
            HP = maxHP;
        }

        if (immuneFrames > 0)
        {
            immuneFrames -= 1;
        }
    }

    void OnCollisionEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && immuneFrames == 0)
        {
            HP -= 0.5f;
            immuneFrames = 50;
        }
    }
}
