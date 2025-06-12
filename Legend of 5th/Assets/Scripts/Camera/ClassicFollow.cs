using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ClassicFollow : MonoBehaviour
{
    public GameObject player;
    private PlayerController playerC;
    public float offsetX;
    public float offsetY;
    public float screenLengthX;
    public float screenLengthY;
    public float cameraSpeed;
    private bool newRoom;
    private Vector3 target;

    public int[] enemiesPerRoom;
    public GameObject keyBat;
    public bool[] keyBatHere;
    public GameObject[] bogEnemies;
    public GameObject[] lavaEnemies;
    public GameObject[] d1Enemies;
    public GameObject[] d2Enemies;
    public GameObject d1Boss;
    public bool d1Beaten;
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
            ResetRoom();

            //snap player into camera when alost done with transition
            if (Vector3.Distance(transform.position, target) < 1)
            {
                if (playerC.transform.position.x - (transform.position.x  - offsetX)< -screenLengthX / 2 + 0.625f)
                {
                    playerC.transform.position = new Vector3(
                        transform.position.x - offsetX - (screenLengthX / 2 - 0.625f),
                        playerC.transform.position.y,
                        playerC.transform.position.z);
                }

                if (playerC.transform.position.x - (transform.position.x - offsetX)> screenLengthX / 2 - 0.625f)
                {
                    playerC.transform.position = new Vector3(
                        transform.position.x - offsetX + (screenLengthX / 2 - 0.625f),
                        playerC.transform.position.y,
                        playerC.transform.position.z);
                }

                if (playerC.transform.position.y - (transform.position.y - offsetY) < -screenLengthY / 2 + 0.5f)
                {
                    playerC.transform.position = new Vector3(
                        playerC.transform.position.x,
                        transform.position.y - offsetY - (screenLengthY / 2 - 0.5f),
                        playerC.transform.position.z);
                }

                if (playerC.transform.position.y - (transform.position.y - offsetY)> screenLengthY / 2 - 0.5f)
                {
                    playerC.transform.position = new Vector3(
                        playerC.transform.position.x,
                        transform.position.y - offsetY + (screenLengthY / 2 - 0.5f),
                        playerC.transform.position.z);
                }
            }
        }

        //double check to see if you just entered a new room
        if (newRoom && !playerC.cameraTransition && (transform.position.x < 50 || transform.position.y < -30))
        {
            int roomID = (int)(
                    Mathf.Floor((transform.position.x - offsetX) / screenLengthX) +
                    Mathf.Floor((transform.position.y - offsetY) / screenLengthY) * 5 + 66);

            if (roomID == 48 && 
                !d1Beaten)
            {
                GameObject instantiatedPoof = Instantiate(poof, new Vector3(40, -46, -6), Quaternion.identity);
                instantiatedPoof.GetComponent<POOF>().spawn = d1Boss;
            }
            else
            {
                int spawnNum = enemiesPerRoom[roomID];

                GameObject[] enemiesToSpawn;
                if (player.transform.position.y < -90)
                {
                    enemiesToSpawn = d2Enemies;
                }
                else if (player.transform.position.y < -30)
                {
                    enemiesToSpawn = d1Enemies;
                }
                else if (player.transform.position.x < -6)
                {
                    enemiesToSpawn = lavaEnemies;
                }
                else
                {
                    enemiesToSpawn = bogEnemies;
                }

                //spawn enemies in new room
                for (int i = 0; i < spawnNum; i++)
                {
                    GameObject instantiatedPoof = Instantiate(poof, GetRandomPos(transform.position), Quaternion.identity);
                    instantiatedPoof.GetComponent<POOF>().spawn = enemiesToSpawn[Random.Range(0, enemiesToSpawn.Length)];
                }

                if (keyBatHere[roomID])
                {
                    GameObject instantiatedPoof = Instantiate(poof, GetRandomPos(transform.position), Quaternion.identity);
                    instantiatedPoof.GetComponent<POOF>().spawn = keyBat;
                }
            }
        }
    }

    private Vector3 GetRandomPos(Vector3 pos)
    {
        Vector3 newPos = new Vector3(
               pos.x - offsetX + Random.Range((int)screenLengthX / 2 - 2, (int)-screenLengthX / 2 + 1),
               pos.y - offsetY + Random.Range((int)screenLengthY / 2 - 2, (int)-screenLengthY / 2 + 1),
               -0.5f);

        while (Vector3.Distance(newPos, player.transform.position) <= 2)
        {
            newPos = new Vector3(
            pos.x - offsetX + Random.Range((int)screenLengthX / 2 - 2, (int)-screenLengthX / 2 + 1),
            pos.y - offsetY + Random.Range((int)screenLengthY / 2 - 2, (int)-screenLengthY / 2 + 1),
            -0.5f);
        }

        return newPos;
    }

    public void ResetRoom()
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
        toDelete = GameObject.FindGameObjectsWithTag("Dust");
        for (int i = 0; i < toDelete.Length; i++)
        {
            Destroy(toDelete[i]);
        }
        toDelete = GameObject.FindGameObjectsWithTag("Projectile");
        for (int i = 0; i < toDelete.Length; i++)
        {
            Destroy(toDelete[i]);
        }
    }
}
