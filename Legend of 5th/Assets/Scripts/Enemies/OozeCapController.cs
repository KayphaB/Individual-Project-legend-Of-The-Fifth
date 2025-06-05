using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OozeCapController : MonoBehaviour
{
    public int Health;

    public float speed;
    public int direction = 1;
    public GameObject[] moveChecks;
    public GameObject player;

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
        //turn red for a bit after getting hit
        if (hitReset > 0)
        {
            hitReset -= 1;
            sr.color = hit;
        }
        else
        {
            sr.color = white;
        }

        if (Health <= 0)
        {
            Destroy(this.gameObject);
        }

        //if the player is lined up with you then switch your direction towards the player if possible
        if (Vector3.Distance(transform.position, player.transform.position) <= 3)
        {
            Debug.Log("player close enough x dist: " + Mathf.Abs(player.transform.position.x - transform.position.x) + " y dist: " + Mathf.Abs(player.transform.position.y - transform.position.y));
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
        if (moveChecks[direction - 1].GetComponent<MoveCheckers>().isColliding)
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        //take damage if collided with weapon, more damage if its the mace
        if (other.CompareTag("Weapon") && hitReset <= 0)
        {
            hitReset = 30;
            Health -= 2;
        }
        else if (other.CompareTag("Weapon+") && hitReset <= 0)
        {
            hitReset = 30;
            Health -= 4;
        }
    }
}
