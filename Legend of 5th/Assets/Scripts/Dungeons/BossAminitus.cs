using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAminitus : MonoBehaviour
{
    public int defences;
    public GameObject[] defenceObjects;
    public int state;

    private GameObject instantiatedPoof;
    public GameObject poof;
    public GameObject[] lootTable;
    public float dropChance;

    private Rigidbody2D rb;
    private GameObject player;
    private Animator anim;
    public float spinSpeed;
    public float degrees;

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
        degrees += spinSpeed * Time.deltaTime;
        if (degrees > 360)
        {
            degrees = 0;
        }

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

        for (int i = 0; i < defenceObjects.Length; i++)
        {
            defenceObjects[i].SetActive(defences > i);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //take damage if collided with weapon, more damage if its the mace
        if (other.CompareTag("Projectile"))
        {
            if (other.name.Contains("Super"))
            {
                Destroy(other.gameObject);
                defences -= 1;
                state = (state + Random.Range(-1, 2) % 3);
            }
        }
    }
}
