using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class POOF : MonoBehaviour
{
    public GameObject spawn;
    private float puffDecayTime;
    public bool puffDecay;

    public Sprite[] sprites;
    private SpriteRenderer sr;
    void Start()
    {
        puffDecayTime = 15;
        sr = GetComponent<SpriteRenderer>();
    }
    void FixedUpdate()
    {
        if (puffDecay)
        {
            puffDecayTime -= 1;
        }

        if (puffDecayTime <= 1)
        {
            Instantiate(spawn, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        sr.sprite = sprites[(int)Mathf.Ceil(puffDecayTime / 5) - 1];
    }
}
