using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;

    private Animator anim;
    public int direction;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        anim.speed = 1;
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(transform.right * Time.deltaTime * -speed);
            direction = 1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(transform.right * Time.deltaTime * speed);
            direction = 2;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(transform.up * Time.deltaTime * speed);
            direction = 3;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(transform.up * Time.deltaTime * -speed);
            direction = 4;
        }
        else
        {
            //if no walking keys pressed, stop animation
            anim.speed = 0;
        }

        //update the animation's variables
        anim.SetInteger("direction", direction);
    }
}
