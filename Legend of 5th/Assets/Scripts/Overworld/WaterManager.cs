using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WaterManager : MonoBehaviour
{
    public bool hasRaft;
    private TilemapCollider2D collider;
    void Start()
    {
        collider = GetComponent<TilemapCollider2D>();
    }

    void Update()
    {
        //disable water collisions if the player has the raft
        collider.isTrigger = (hasRaft);
    }
}
