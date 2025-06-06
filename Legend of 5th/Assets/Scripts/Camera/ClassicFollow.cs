using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ClassicFollow : MonoBehaviour
{
    private GameObject player;
    private PlayerController playerC;
    public float offsetX;
    public float offsetY;
    public float screenLengthX;
    public float screenLengthY;
    public float cameraSpeed;
    private bool newRoom;
    private Vector3 target;

    public GameObject[] enemiesToSpawn;
    public GameObject poof;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerC = player.GetComponent<PlayerController>();
    }

    void Update()
    {
        //find target (the screen the player is in and the camera should be)
        target = new Vector3(
            Mathf.Round(player.transform.position.x / screenLengthX) * screenLengthX + offsetX,
            Mathf.Round(player.transform.position.y / screenLengthY) * screenLengthY + offsetY,
        -10);
    }

    private void FixedUpdate()
    {
        newRoom = playerC.cameraTransition;

        playerC.cameraTransition = false;

        //check to see if camera is very close to the target
        if (Vector3.Distance(transform.position, target) < cameraSpeed)
        {
            transform.position = target;
        }

        //correct camera X position if not equal to target
        if (transform.position.x < target.x)
        {
            playerC.cameraTransition = true;
            transform.position = new Vector3(
                transform.position.x + cameraSpeed,
                transform.position.y,
                transform.position.z);
        }
        else if (transform.position.x > target.x)
        {
            playerC.cameraTransition = true;
            transform.position = new Vector3(
                transform.position.x - cameraSpeed,
                transform.position.y,
                transform.position.z);
        }

        //correct camera Y position if not equal to target
        if (transform.position.y < target.y)
        {
            playerC.cameraTransition = true;
            transform.position = new Vector3(
                transform.position.x,
                transform.position.y + cameraSpeed,
                transform.position.z);
        }
        else if (transform.position.y > target.y)
        {
            playerC.cameraTransition = true;
            transform.position = new Vector3(
                transform.position.x,
                transform.position.y - cameraSpeed,
                transform.position.z);
        }

        //check to see if you're in the middle of a screen transition
        if (playerC.cameraTransition)
        {
            //remove all enemies and pickups from previous room
            GameObject[] toDelete = GameObject.FindGameObjectsWithTag("Enemy");
            for (int i = 0; i < toDelete.Length; i++)
            {
                Destroy(toDelete[i]);
            }
            toDelete = GameObject.FindGameObjectsWithTag("Pickup");
            for (int i = 0; i < toDelete.Length; i++)
            {
                Destroy(toDelete[i]);
            }
        }

        //double check to see if you just entered a new room
        if (newRoom && !playerC.cameraTransition)
        {
            //spawn enemies in new room
            for (int i = 0;i < 3;i++)
            {
                GameObject instantiatedPoof = Instantiate(poof, GetRandomPos(transform.position), Quaternion.identity);
                instantiatedPoof.GetComponent<POOF>().spawn = enemiesToSpawn[0];
            }
        }
    }

    private Vector3 GetRandomPos(Vector3 pos)
    {
        return new Vector3(
            pos.x + Random.Range((int) screenLengthX / 2 - 2, (int) -screenLengthX / 2 + 1),
            pos.y + Random.Range((int) screenLengthY / 2 - 2, (int) -screenLengthY / 2 + 1),
            -0.5f);
    }
}
