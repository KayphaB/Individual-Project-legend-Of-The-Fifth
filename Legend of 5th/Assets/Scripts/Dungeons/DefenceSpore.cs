using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenceSpore : MonoBehaviour
{
    public Sprite[] stateSprites;
    private SpriteRenderer sr;

    public float radius;
    public float offset;
    void Start()
    {
        sr =GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float degrees = transform.parent.GetComponent<BossAminitus>().degrees;
        transform.position = new Vector3(
            transform.parent.transform.position.x + Mathf.Cos((degrees + offset) * Mathf.Deg2Rad) * radius,
            transform.parent.transform.position.y + Mathf.Sin((degrees + offset) * Mathf.Deg2Rad) * radius, 
            -5);

        int state = transform.parent.GetComponent<BossAminitus>().state;
        sr.sprite = stateSprites[state];
    }
}
