using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SporeBlast : MonoBehaviour
{
    public int direction;
    public float speed;
    public float damage;
    public bool blockable;
    private PlayerController player;

    private Rigidbody2D rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    void FixedUpdate()
    {
        if (!player.openInventory)
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        //take damage if collided with player, deal damage. if collided with Environment, delete itself
        if (other.CompareTag("Player"))
        {
            if ((direction < 3) == (player.direction < 3) && 
                player.direction != direction && 
                player.gameObject.GetComponent<ChainController>().length < 0.5f && 
                blockable)
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
            else
            {
                PlayerHealth ph = other.transform.GetChild(0).GetComponent<PlayerHealth>();
                ph.HP -= damage;
                ph.immuneFrames = 40;
                ph.hitFrames = 10;
                Destroy(gameObject);
            }
        }
        else if (other.CompareTag("Environment"))
        {
            Destroy(gameObject);
        }
    }
}
