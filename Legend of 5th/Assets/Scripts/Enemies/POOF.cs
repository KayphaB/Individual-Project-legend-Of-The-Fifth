using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class POOF : MonoBehaviour
{
    public GameObject spawn;
    private float puffDecayTime;
    public bool puffDecay;

    public Sprite[] sprites;
    private SpriteRenderer sr;

    private ClassicFollow cameraS;
    private Vector2 checkSize = Vector2.one;
    void Start()
    {
        puffDecayTime = 15;
        sr = GetComponent<SpriteRenderer>();
        cameraS = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ClassicFollow>();

        while (IsColliding())
        {
            transform.position = new Vector3(
                cameraS.transform.position.x + Random.Range((int)cameraS.screenLengthX / 2 - 2, (int)-cameraS.screenLengthX / 2 + 1),
                cameraS.transform.position.y + Random.Range((int)cameraS.screenLengthY / 2 - 2, (int)-cameraS.screenLengthY / 2 + 1),
                -0.5f);
        }
    }
    void FixedUpdate()
    {
        if (puffDecay)
        {
            puffDecayTime -= 1;
        }

        if (puffDecayTime <= 1)
        {
            if (spawn != null)
            {
                Instantiate(spawn, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }

        sr.sprite = sprites[(int)Mathf.Ceil(puffDecayTime / 5) - 1];
    }

    bool IsColliding()
    {
        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, checkSize, 0);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].CompareTag("Environment"))
            {
                return true;
            }
        }

        return false;
    }
}
