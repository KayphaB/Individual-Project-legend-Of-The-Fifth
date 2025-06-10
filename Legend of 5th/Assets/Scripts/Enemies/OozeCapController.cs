using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OozeCapController : MonoBehaviour
{
    public int Health;
    private GameObject instantiatedPoof;
    public GameObject poof;
    public GameObject[] lootTable;
    public float dropChance;

    public float speed;
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
        for (int i = 0;i < 4;i++)
        {
            moveChecks[i] = moveCheckObjects[i].GetComponent<MoveCheckers>();
        }
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
                instantiatedPoof = Instantiate(poof, transform.position, Quaternion.identity);
                if (dropChance >= Random.Range(0.0f, 1.0f))
                {
                    instantiatedPoof.GetComponent<POOF>().spawn = lootTable[Random.Range(0, lootTable.Length)];
                }
                else
                {
                    instantiatedPoof.GetComponent<POOF>().spawn = null;
                }
                Destroy(this.gameObject);
            }

            //if the player is lined up with you then switch your direction towards the player if possible
            //if (Vector3.Distance(transform.position, player.transform.position) <= 2.5)
            //{
            //    if (Mathf.Abs(player.transform.position.x - transform.position.x) < 1 && (direction == 1 || direction == 2))
            //   {
            //        if (player.transform.position.y < transform.position.y &&
            //            (!moveChecks[3].isColliding && !moveChecks[3].outOfBounds))
            //        {
            //            direction = 4;
            //        }
            //        else if (!moveChecks[2].isColliding && !moveChecks[2].outOfBounds)
            //        {
            //            direction = 3;
            //        }
            //    }
            //
            //    if (Mathf.Abs(player.transform.position.y - transform.position.y) < 1 && (direction == 3 || direction == 4))
            //    {
            //        if (player.transform.position.x < transform.position.x &&
            //            (!moveChecks[0].isColliding && !moveChecks[0].outOfBounds))
            //        {
            //            direction = 1;
            //        }
            //        else if (!moveChecks[1].isColliding && !moveChecks[1].outOfBounds)
            //        {
            //            direction = 2;
            //        }
            //    }
            //}

            //If the direction your moving in is blocked than switch directions
            if (moveChecks[direction - 1].isColliding || moveChecks[direction - 1].outOfBounds)
            {
                if (Random.Range(0, 2) == 1)
                {
                    if (direction == 1 || direction == 3)
                    {
                        direction += 2;
                        if (direction > 4)
                        {
                            direction = 1;
                        }
                    }
                    else
                    {
                        direction += 1;
                        if (direction > 4)
                        {
                            direction = 1;
                        }
                    }
                }
                else
                {
                    if (direction == 1 || direction == 3)
                    {
                        direction += -1;
                        if (direction < 1)
                        {
                            direction = 4;
                        }
                    }
                    else
                    {
                        direction += -2;
                        if (direction < 1)
                        {
                            direction = 4;
                        }
                    }
                }
            }

            //constantly move in the set direction unless stuck in the wall
            if (!moveChecks[direction - 1].isColliding && !moveChecks[direction - 1].outOfBounds)
            {
                if (direction == 1)
                {
                    rb.MovePosition(transform.position + new Vector3(-speed * Time.deltaTime, 0, 0));
                }
                else if (direction == 2)
                {
                    rb.MovePosition(transform.position + new Vector3(speed * Time.deltaTime, 0, 0));
                }
                else if (direction == 3)
                {
                    rb.MovePosition(transform.position + new Vector3(0, speed * Time.deltaTime, 0));
                }
                else if (direction == 4)
                {
                    rb.MovePosition(transform.position + new Vector3(0, -speed * Time.deltaTime, 0));
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
            Health -= 6;
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
