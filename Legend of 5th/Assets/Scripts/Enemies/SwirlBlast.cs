using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwirlBlast : MonoBehaviour
{
    private Vector3 direction;
    public float speed;
    public float damage;
    private PlayerController player;
    public bool destroyOnEnvironment;

    private Rigidbody2D rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        //set direction relative to the player
        direction = (player.transform.position - transform.position).normalized;
    }

    void FixedUpdate()
    {
        //move in the set direction
        if (!player.openInventory)
        {
            rb.MovePosition(transform.position + direction.normalized * speed);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //take damage if collided with player, deal damage. if collided with Environment, delete itself
        if (other.CompareTag("Player"))
        {
            PlayerHealth ph = other.transform.GetChild(0).GetComponent<PlayerHealth>();
            ph.HP -= damage;
            ph.immuneFrames = 40;
            ph.hitFrames = 10;
            Destroy(gameObject);
        }
        else if (other.CompareTag("Environment") && destroyOnEnvironment)
        {
            Destroy(gameObject);
        }
    }
}

