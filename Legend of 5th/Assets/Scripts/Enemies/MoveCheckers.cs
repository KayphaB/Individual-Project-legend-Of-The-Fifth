using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCheckers : MonoBehaviour
{
    public bool isColliding;
    public bool outOfBounds;
    private GameObject camera;
    private void Start()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera");
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Environment"))
        {
            isColliding = true;
        }
        else
        {
            isColliding = false;
        }
    }

    void Update()
    {
        outOfBounds = false;

        //trigger a collision if enemy goes off camera
        if (Mathf.Abs(transform.position.x - camera.transform.position.x) > camera.GetComponent<ClassicFollow>().screenLengthX / 2 - GetComponent<BoxCollider2D>().size.x / 2)
        {
            outOfBounds = true;
        }

        if (Mathf.Abs(transform.position.y - camera.transform.position.y) > camera.GetComponent<ClassicFollow>().screenLengthY / 2 - GetComponent<BoxCollider2D>().size.y / 2)
        {
            outOfBounds = true;
        }
    }
}
