using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAminitus : MonoBehaviour
{
    public int Health;
    private GameObject instantiatedPoof;
    public GameObject poof;
    public GameObject[] lootTable;
    public float dropChance;

    private Rigidbody2D rb;
    private GameObject player;
    private Animator anim;

    private float hitReset;
    private float colorShift;
    public Color hit;
    public Color white;
    private SpriteRenderer sr;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (!player.GetComponent<PlayerController>().openInventory)
        {
            //turn red for a bit after getting hit
            if (colorShift > 0)
            {
                colorShift -= 1;
                sr.color = hit;
            }
            else
            {
                sr.color = white;
            }

            //decay hitReset over time
            if (hitReset > 0)
            {
                hitReset -= 1;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //take damage if collided with weapon, more damage if its the mace
        if (other.CompareTag("Weapon") && hitReset <= 0)
        {
            Health -= 2;
            hitReset = 30;
            colorShift = 5;
        }
        else if (other.CompareTag("Weapon+") && hitReset <= 0)
        {
            Health -= 4;
            hitReset = 30;
            colorShift = 5;
        }
        else if (other.CompareTag("Explosion") && hitReset <= 0)
        {
            Health -= 10;
            hitReset = 30;
            colorShift = 5;
        }
        else if (other.CompareTag("Projectile") && hitReset <= 0)
        {
            Health -= (int)other.GetComponent<Projectile>().damage;
            Destroy(other.gameObject);
            hitReset = 30;
            colorShift = 5;
        }
    }
}
