using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int direction;
    public float speed;
    public float damage;
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
}
