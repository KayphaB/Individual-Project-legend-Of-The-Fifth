using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHP = 3;
    public float HP = 3;
    
    public float immuneFrames;

    public float hitFrames;
    private SpriteRenderer sr;
    public Color white;
    public Color onHit;
    private void Start()
    {
        sr = transform.parent.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        //cap health at the max
        if (HP > maxHP)
        {
            HP = maxHP;
        }
    }

    private void FixedUpdate()
    {
        if (hitFrames > 0)
        {
            hitFrames -= 1;
            sr.color = onHit;
        }
        else
        {
            sr.color = white;
        }

        if (immuneFrames > 0)
        {
            immuneFrames -= 1;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && immuneFrames == 0)
        {
            HP -= 0.5f;
            immuneFrames = 40;
            hitFrames = 10;
        }
    }
}
