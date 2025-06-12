using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class GlimmerShroomController : MonoBehaviour
{
    public int Health;
    private GameObject instantiatedPoof;
    public GameObject poof;
    public GameObject[] lootTable;
    public float dropChance;

    private ClassicFollow cameraS;
    private Vector2 checkSize = Vector2.one * 0.7f;

    public float stateShift;
    public float framesPerTeleport;
    public float framesPerState;
    public Sprite[] states;
    private float hitReset;
    private float colorShift;
    public Color hit;
    public Color white;
    private SpriteRenderer sr;

    public GameObject projectile;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        cameraS= GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ClassicFollow>();
        MoveToOpen();
    }

    void FixedUpdate()
    {
        //turn red for a bit after getting hit
        if (colorShift > 0)
        {
            colorShift -= 1;
            sr.color = hit;
        }
        else
        {
            sr.color = white;
        }

        //decay hitReset over time
        if (hitReset > 0)
        {
            hitReset -= 1;
        }

        //die if health reaches zero
        if (Health <= 0)
        {
            instantiatedPoof = Instantiate(poof, transform.position, Quaternion.identity);
            if (dropChance >= Random.Range(0.0f, 1.0f))
            {
                instantiatedPoof.GetComponent<POOF>().spawn = lootTable[Random.Range(0, lootTable.Length)];
            }
            else
            {
                instantiatedPoof.GetComponent<POOF>().spawn = null;
            }
            Destroy(this.gameObject);
        }

        //increase stateShift to move naimation along
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().inven)
        {
            stateShift++;
        }
        if (stateShift > framesPerTeleport)
        {
            stateShift = -framesPerTeleport;
            Instantiate(projectile, transform.position, Quaternion.identity);
        }

        //set proper sprite for frame in animation
        if (Mathf.Abs(stateShift) + 10 > framesPerTeleport)
        {
            sr.sprite = states[states.Length - 1];
        }
        else if (Mathf.Abs(stateShift) > framesPerState * (states.Length - 2))
        {
            sr.sprite = states[states.Length - 2];
        }
        else
        {
            sr.sprite = states[(int) (Mathf.Floor(Mathf.Abs(stateShift) / framesPerState))];
        }

        if (stateShift == 0)
        {
            Teleport();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //take damage if collided with weapon, more damage if its the mace
        if (other.CompareTag("Weapon") && hitReset <= 0 && Mathf.Abs(stateShift) > 2 * framesPerState)
        {
            Health -= 2;
            hitReset = 30;
            colorShift = 5;
            if (Mathf.Abs(stateShift) > framesPerState * (states.Length - 2))
            {
                stateShift = -framesPerState * (states.Length - 2);
            }

        }
        else if (other.CompareTag("Weapon+") && hitReset <= 0)
        {
            Health -= 4;
            hitReset = 30;
            colorShift = 5;
            if (Mathf.Abs(stateShift) > framesPerState * (states.Length - 2))
            {
                stateShift = -framesPerState * (states.Length - 2);
            }
        }
        else if (other.CompareTag("Explosion") && hitReset <= 0)
        {
            Health -= 6;
            hitReset = 30;
            colorShift = 5;
            if (Mathf.Abs(stateShift) > framesPerState * (states.Length - 2))
            {
                stateShift = -framesPerState * (states.Length - 2);
            }
        }
        else if (other.CompareTag("Projectile") && hitReset <= 0)
        {
            Health -= (int)other.GetComponent<Projectile>().damage;
            Destroy(other.gameObject);
            hitReset = 30;
            colorShift = 5;
            if (Mathf.Abs(stateShift) > framesPerState * (states.Length - 2))
            {
                stateShift = -framesPerState * (states.Length - 2);
            }
        }
    }

    //Teleport to a new, random, avalible position
    private void Teleport()
    {
        transform.position = new Vector3(
            cameraS.transform.position.x - cameraS.offsetX + 0.5f + Random.Range((int)cameraS.screenLengthX / 2 - 2, (int)-cameraS.screenLengthX / 2 + 1),
            cameraS.transform.position.y - cameraS.offsetY + 0.5f + Random.Range((int)cameraS.screenLengthY / 2 - 2, (int)-cameraS.screenLengthY / 2 + 1),
            -0.5f);

        MoveToOpen();
    }

    //teleport wround randomly until no collisions with the "Environment" are detected
    private void MoveToOpen()
    {
        while (IsColliding())
        {
            transform.position = new Vector3(
                cameraS.transform.position.x - cameraS.offsetX + 0.5f + Random.Range((int)cameraS.screenLengthX / 2 - 2, (int)-cameraS.screenLengthX / 2 + 1),
                cameraS.transform.position.y - cameraS.offsetY + 0.5f + Random.Range((int)cameraS.screenLengthY / 2 - 2, (int)-cameraS.screenLengthY / 2 + 1),
                -0.5f);
        }
    }

    bool IsColliding()
    {
        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, checkSize, 0);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].CompareTag("Environment") || hits[i].CompareTag("Player"))
            {
                return true;
            }
        }

        return false;
    }
}
