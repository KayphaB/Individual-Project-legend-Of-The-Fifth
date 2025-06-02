using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHP = 3;
    public float HP = 3;
    
    void Update()
    {
        //cap health at the max
        if (HP > maxHP)
        {
            HP = maxHP;
        }
    }
}
