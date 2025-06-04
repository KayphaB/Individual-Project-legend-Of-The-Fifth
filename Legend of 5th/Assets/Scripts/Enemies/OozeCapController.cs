using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OozeCapController : MonoBehaviour
{
    public int Health;
    public float speed;

    private float hitReset;
    public Color hit;
    public Color white;
    private SpriteRenderer sr;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        if (hitReset > 0)
        {
            hitReset -= 1;
            sr.color = hit;
        }
        else
        {
            sr.color = white;
        }
    }

    void OnTriggerEnter(Collider2D collider)
    {
        if (other.tag == "Weapon" && hitReset <= 0)
        {
            hitReset = 30;
            Health -= 2;
        }
        else if (other.tag == "Weapon+" && hitReset <= 0)
        {
            hitReset = 30;
            Health -= 4;
        }
    }
}
