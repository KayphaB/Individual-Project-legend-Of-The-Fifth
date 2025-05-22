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
        StartCoroutine(Flip());
    }

    IEnumerator Flip()
    {
        while(true)
        {
            yield return new WaitForSeconds(speed);

            //flip the fire
            sprite.flipX = true;

            yield return new WaitForSeconds(speed);

            //flip the fire back
            sprite.flipX = false; 
        }
    }
}
