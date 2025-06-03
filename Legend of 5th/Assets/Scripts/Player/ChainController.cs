using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainController : MonoBehaviour
{
    private PlayerController pc;
    private bool extending;
    public int weapon;
    public float length;
    public float chainSpeed;
    public float maceSpeed;

    public GameObject[] chains;
    public GameObject[] maces;

    public Sprite[] chainSprites;
    public Sprite[] maceSprites;
    void Start()
    {
        pc = GetComponent<PlayerController>();
    }

    void Update()
    {
        if (weapon != 0)
        {
            //start extending weapon when J is pressed
            if (Input.GetKeyDown(KeyCode.J))
            {
                if (length == 0)
                {
                    extending = true;
                    Debug.Log("extending");
                }
                Debug.Log("J pressed");
            }
            
            //retract weapon when J is released or maximum length is reached
            if (Input.GetKeyUp(KeyCode.J) || length >= 2 && weapon == 1 || length >= 4 && weapon == 2)
            {
                extending = false;
            }
        }

        if (length > 0)
        {
            pc.usingWeapon = true;
        }
        else
        {
            pc.usingWeapon = false;
        }
    }

    void FixedUpdate()
    {
        if (weapon != 0)
        {
            //increase weapon legth when extending and decrease it when retracting
            if (extending)
            {
                if (weapon == 1)
                {
                    length += chainSpeed;
                }
                else
                {
                    length += maceSpeed;
                }
            }
            else
            {
                if (weapon == 1)
                {
                    length -= chainSpeed;
                }
                else
                {
                    length -= maceSpeed;
                }

                if (length < 0)
                {
                    length = 0;
                }
            }

            //set all weapons inactive then only activate the current weapon (direction) and change it's sprite accordingly
            for (int i = 0;i < 4;i++)
            {
                chains[i].SetActive(false);
                maces[i].SetActive(false);
            }
            if (length >= 0.5f)
            {
                if (weapon == 1)
                {
                    chains[pc.direction - 1].SetActive(true);
                    chains[pc.direction - 1].GetComponent<SpriteRenderer>().sprite = chainSprites[(int) (Mathf.Round(length * 2) - 1)];
                }
                else
                {
                    maces[pc.direction - 1].SetActive(true);
                    maces[pc.direction - 1].GetComponent<SpriteRenderer>().sprite = maceSprites[(int) (Mathf.Round(length * 2) - 1)];
                }
            }
        }
    }
}
