using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnlockablesManager : MonoBehaviour
{
    public GameObject[] unlockablesVisuals;
    public bool[] unlockables;

    public WaterManager water;
    public WaterManager lava;
    public ChainController chain;

    public GameObject weaponUsedJ;
    public Color clear;
    public Color white;
    public Sprite[] weapons;
    private void Start()
    {
        chain = GameObject.FindGameObjectWithTag("Player").GetComponent<ChainController>();
    }
    void Update()
    {
        //show and hide the appropriate items
        for (int i = 0; i < unlockablesVisuals.Length; i++)
        {
            if (unlockables[i])
            {
                unlockablesVisuals[i].SetActive(true);
            }
            else
            {
                unlockablesVisuals[i].SetActive(false);
            }
        }

        //set lava and water avalable to walkable
        water.hasRaft = unlockables[3];
        lava.hasRaft = unlockables[4];

        //set appropriate weapon
        weaponUsedJ.GetComponent<Image>().color = white;
        if (unlockables[1])
        {
            chain.weapon = 2;
            unlockables[0] = false;
            unlockables[2] = false;
        }
        else if (unlockables[0])
        {
            chain.weapon = 1;
        }
        else
        {
            chain.weapon = 0;
            weaponUsedJ.GetComponent<Image>().color = clear;
        }

        weaponUsedJ.GetComponent<Image>().sprite = weapons[chain.weapon];
    }
}
