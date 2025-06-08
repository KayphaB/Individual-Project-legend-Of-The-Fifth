using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCheckers : MonoBehaviour
{
    public bool isColliding;
    private int detected;

    public bool outOfBounds;
    private ClassicFollow camera;
    private BoxCollider2D collider;
    private void Start()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ClassicFollow>();
        collider = GetComponent<BoxCollider2D>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Environment"))
        {
            detected++;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Environment"))
        {
            detected--;
        }
    }

    void Update()
    {
        //calculate whether lower left corner OR upper right corner is out of bounds
        Vector3 downLeft = new Vector3 (
            transform.position.x + collider.offset.x - collider.size.x / 2 - camera.transform.position.x, 
            transform.position.y + collider.offset.y - collider.size.y / 2 - camera.transform.position.y + 2, 
            transform.position.z);

        Vector3 upRight = new Vector3(
            transform.position.x + collider.offset.x + collider.size.x / 2 - camera.transform.position.x,
            transform.position.y + collider.offset.y + collider.size.y / 2 - camera.transform.position.y + 2,
            transform.position.z);

        outOfBounds = (
            Mathf.Abs(upRight.x) > camera.screenLengthX / 2 ||
            Mathf.Abs(upRight.y) > camera.screenLengthY / 2 ||
            Mathf.Abs(downLeft.x) > camera.screenLengthX / 2 ||
            Mathf.Abs(downLeft.y) > camera.screenLengthY / 2);

        //check if your colidign with any colliders named "Environment"
        isColliding = detected > 0;
    }
}
