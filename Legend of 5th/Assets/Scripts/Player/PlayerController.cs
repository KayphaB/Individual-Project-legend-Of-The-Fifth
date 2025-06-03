using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public bool frozen;
    public bool enteringCave;
    public bool usingWeapon;

    private Animator anim;
    public int direction;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        frozen = enteringCave || usingWeapon;

        anim.speed = 1;
        if (Input.GetKey(KeyCode.A) && !frozen)
        {
            transform.Translate(transform.right * Time.deltaTime * -speed);
            direction = 1;
        }
        else if (Input.GetKey(KeyCode.D) && !frozen)
        {
            transform.Translate(transform.right * Time.deltaTime * speed);
            direction = 2;
        }
        else if (Input.GetKey(KeyCode.W) && !frozen)
        {
            transform.Translate(transform.up * Time.deltaTime * speed);
            direction = 3;
        }
        else if (Input.GetKey(KeyCode.S) && !frozen)
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
