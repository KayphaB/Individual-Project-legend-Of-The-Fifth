using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SporeBudController : MonoBehaviour
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

    public GameObject sporeBlast;
    private float blastCooldown = 50;
    public float blastDelay;
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
        direction = Random.Range(1, 5);
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        for (int i = 0; i < 4; i++)
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

            //decay blastCooldown over time
            if (blastCooldown > 0)
            {
                blastCooldown -= 1;
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

            //if the direction your moving is colliding then turn around
            if (moveChecks[direction - 1].isColliding || moveChecks[direction - 1].outOfBounds)
            {
                if (direction == 1 || direction == 3)
                {
                    direction += 1;
                }
                else
                {
                    direction -= 1;
                }
            }

            //constantly move in the set direction unless stuck in the wall
            if (!moveChecks[direction - 1].isColliding && !moveChecks[direction - 1].outOfBounds && blastCooldown <= blastDelay - 20)
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

            //if the player is lined up with you and the blastCooldown is zero then blast the player
            if (blastCooldown == 0)
            {
                if (Mathf.Abs(player.transform.position.x - transform.position.x) < 1 && (direction == 1 || direction == 2))
                {
                    if (player.transform.position.y < transform.position.y &&
                        (!moveChecks[3].isColliding && !moveChecks[3].outOfBounds))
                    {
                        GameObject instaSpore = Instantiate(sporeBlast,
                            new Vector3(
                                transform.position.x,
                                transform.position.y - 0.75f,
                                transform.position.z),
                            Quaternion.identity);
                        instaSpore.GetComponent<SporeBlast>().direction = 4;
                    }
                    else if (!moveChecks[2].isColliding && !moveChecks[2].outOfBounds)
                    {
                        GameObject instaSpore = Instantiate(sporeBlast,
                            new Vector3(
                                transform.position.x,
                                transform.position.y + 0.75f,
                                transform.position.z),
                            Quaternion.identity);
                        instaSpore.GetComponent<SporeBlast>().direction = 3;
                    }

                    blastCooldown = blastDelay;
                }

                if (Mathf.Abs(player.transform.position.y - transform.position.y) < 1 && (direction == 3 || direction == 4))
                {
                    if (player.transform.position.x < transform.position.x &&
                        (!moveChecks[0].isColliding && !moveChecks[0].outOfBounds))
                    {
                        GameObject instaSpore = Instantiate(sporeBlast,
                            new Vector3(
                                transform.position.x - 0.75f,
                                transform.position.y,
                                transform.position.z),
                            Quaternion.identity);
                        instaSpore.GetComponent<SporeBlast>().direction = 1;
                    }
                    else if (!moveChecks[1].isColliding && !moveChecks[1].outOfBounds)
                    {
                        GameObject instaSpore = Instantiate(sporeBlast,
                            new Vector3(
                                transform.position.x + .75f,
                                transform.position.y,
                                transform.position.z),
                            Quaternion.identity);
                        instaSpore.GetComponent<SporeBlast>().direction = 2;
                    }

                    blastCooldown = blastDelay;
                }
            }

            //set the animation
            anim.SetBool("blasting", blastCooldown >= blastDelay - 20);
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
    }
}
