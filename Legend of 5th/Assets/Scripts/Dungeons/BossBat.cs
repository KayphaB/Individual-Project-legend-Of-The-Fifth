using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBat : MonoBehaviour
{
    public int Health;
    private GameObject instantiatedPoof;
    public GameObject poof;
    public GameObject[] lootTable;
    public float dropChance;

    private float speedChange;
    public float speedChangeFreq;
    public float maxSpeed;
    public float minSpeed;
    private Animator anim;
    public GameObject summonBat;

    public int direction = 1;
    private Rigidbody2D rb;
    public GameObject[] moveCheckObjects;
    public MoveCheckers[] moveChecks;
    private GameObject player;

    private float hitReset;
    private float colorShift;
    public Color hit;
    public Color white;
    private SpriteRenderer sr;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
        direction = Random.Range(1, 5);
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        for (int i = 0; i < 4; i++)
        {
            moveChecks[i] = moveCheckObjects[i].GetComponent<MoveCheckers>();
        }

        transform.position = new Vector3(
            transform.position.x, 
            transform.position.y, 
            -6);
    }

    private void FixedUpdate()
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

            //die if health reaches zero
            if (Health <= 0)
            {
                GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ClassicFollow>().d1Beaten = true;
                instantiatedPoof = Instantiate(poof, transform.position, Quaternion.identity);
                if (dropChance >= Random.Range(0.0f, 1.0f))
                {
                    instantiatedPoof.GetComponent<POOF>().spawn = lootTable[Random.Range(0, lootTable.Length)];
                }
                else
                {
                    instantiatedPoof.GetComponent<POOF>().spawn = null;
                }
                Destroy(GameObject.FindGameObjectWithTag("Dungeon 1 Door"));
                Destroy(this.gameObject);
            }

            //finds the other direction checker for the diagnal
            int altDirection = direction;
            if (direction < 3)
            {
                altDirection = direction + 2;
            }
            else if (direction == 3)
            {
                altDirection = 2;
            }
            else
            {
                altDirection = 1;
            }

            //change speed
            speedChange += speedChangeFreq;
            if (speedChange >= 1 - (maxSpeed / 100 * minSpeed))
            {
                speedChange = -(1 - (maxSpeed / 100 * minSpeed));
            }
            if (Mathf.Abs(speedChange) <= speedChangeFreq / 2)
            {
                for (int i = 0;i < 3;i++)
                {
                    Instantiate(summonBat, transform.position, Quaternion.identity);
                }
            }
            float speed = maxSpeed * (Mathf.Abs(speedChange) + (maxSpeed / 100) * minSpeed);

            anim.speed = speed;

            //constantly move in the set direction unless stuck in the wall
            if (!moveChecks[direction - 1].outOfBounds && 
                !moveChecks[altDirection - 1].outOfBounds)
            {
                if (direction == 1)
                {
                    rb.MovePosition(transform.position + new Vector3(-speed * Time.deltaTime, speed * Time.deltaTime, 0));
                }
                else if (direction == 2)
                {
                    rb.MovePosition(transform.position + new Vector3(speed * Time.deltaTime, -speed * Time.deltaTime, 0));
                }

                if (direction == 3)
                {
                    rb.MovePosition(transform.position + new Vector3(speed * Time.deltaTime, speed * Time.deltaTime, 0));
                }
                else if (direction == 4)
                {
                    rb.MovePosition(transform.position + new Vector3(-speed * Time.deltaTime, -speed * Time.deltaTime, 0));
                }
            }

            //find if colliding and bounce
            if (moveChecks[direction - 1].outOfBounds)
            {
                direction = altDirection;
            }
            else if (moveChecks[altDirection - 1].outOfBounds)
            {
                if (direction > 2)
                {
                    direction -= 2;
                }
                else if (direction == 2)
                {
                    direction = 3;
                }
                else
                {
                    direction = 4;
                }
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
        else if (other.CompareTag("Dust"))
        {
            Health -= 10;
            Destroy(other.gameObject);
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
