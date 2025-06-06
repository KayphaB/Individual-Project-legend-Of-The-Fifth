using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OozeCapController : MonoBehaviour
{
    public int Health;
    private GameObject instantiatedPoof;
    public GameObject poof;
    public GameObject[] lootTable;

    public float speed;
    public int direction = 1;
    public GameObject[] moveChecks;
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
    }

    private void FixedUpdate()
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

        //constantly move in the set direction
        if (direction == 1)
        {
            transform.position = new Vector3(
                transform.position.x - speed,
                transform.position.y,
                transform.position.z);
        }
        else if (direction == 2)
        {
            transform.position = new Vector3(
                transform.position.x + speed,
                transform.position.y,
                transform.position.z);
        }
        else if (direction == 3)
        {
            transform.position = new Vector3(
                transform.position.x,
                transform.position.y + speed,
                transform.position.z);
        }
        else if (direction == 4)
        {
            transform.position = new Vector3(
                transform.position.x,
                transform.position.y - speed,
                transform.position.z);
        }
    }

    void Update()
    {
        //die if health reaches zero
        if (Health <= 0)
        {
            instantiatedPoof = Instantiate(poof, transform.position, Quaternion.identity);
            instantiatedPoof.GetComponent<POOF>().spawn = lootTable[Random.Range(0, lootTable.Length)];
            Destroy(this.gameObject);
        }

        //if the player is lined up with you then switch your direction towards the player if possible
        if (Vector3.Distance(transform.position, player.transform.position) <= 2.5)
        {
            if (Mathf.Abs(player.transform.position.x - transform.position.x) < 1 && (direction == 1 || direction == 2))
            {
                if (player.transform.position.y < transform.position.y)
                {
                    direction = 4;
                }
                else
                {
                    direction = 3;
                }
            }

            if (Mathf.Abs(player.transform.position.y - transform.position.y) < 1 && (direction == 3 || direction == 4))
            {
                if (player.transform.position.x < transform.position.x)
                {
                    direction = 1;
                }
                else
                {
                    direction = 2;
                }
            }
        }

        //If the direction your moving in is blocked than switch directions
        if (moveChecks[direction - 1].GetComponent<MoveCheckers>().isColliding || moveChecks[direction - 1].GetComponent<MoveCheckers>().outOfBounds)
        {
            if (Random.Range(0, 2) == 1)
            {
                direction += 1;
                if (direction > 4)
                {
                    direction = 1;
                }
            }
            else
            {   
                direction += -1;
                if (direction < 1)
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
    }
}
