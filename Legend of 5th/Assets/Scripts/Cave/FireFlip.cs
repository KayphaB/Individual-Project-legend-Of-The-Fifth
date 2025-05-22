using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFlip : MonoBehaviour
{
    private SpriteRenderer sprite;
    public float speed;
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        StartCoRrutine(FireFlip());
    }

    IEnumerator FireFlip()
    {
        while(true)
        {
            yield return new WaitForSeconds(speed);

            //flip the fire
            sprite.flipY = !sprite.flipY;
        }
    }
}
