using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ActiveShromb : MonoBehaviour
{
    public GameObject Poof;
    private PlayerController pc;
    public float countdown;
    void Start()
    {
        pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    void FixedUpdate()
    {
        //countdown over time, when countdown goes below zero then explode
        if (!pc.openInventory)
        {
            countdown--;
            if (countdown < 0)
            {
                GameObject instapoof = Instantiate(Poof, transform.position, Quaternion.identity);
                instapoof.GetComponent<POOF>().outOfEnvironment = false;
                for (int i = 0; i < 4;i++)
                {
                    Vector3 spawnPos = Vector3.one;
                    if (i < 2)
                    {
                        spawnPos = new Vector3(
                            transform.position.x + Random.Range(-1f, 1f),
                            transform.position.y + 1 - 2 * i,
                            transform.position.z);
                    }
                    else
                    {
                        spawnPos = new Vector3(
                            transform.position.x - 1 + 2 * (i - 2),
                            transform.position.y + Random.Range(-1f, 1f),
                            transform.position.z);
                    }

                    instapoof = Instantiate(Poof, spawnPos, Quaternion.identity);
                    instapoof.GetComponent<POOF>().outOfEnvironment = false;
                }

                Destroy(gameObject);
            }
        }
    }
}
