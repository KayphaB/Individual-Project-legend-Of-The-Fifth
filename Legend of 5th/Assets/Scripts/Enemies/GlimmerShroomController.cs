using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlimmerShroomController : MonoBehaviour
{
    private ClassicFollow cameraS;
    private Vector2 checkSize = Vector2.one * 0.7f;

    private float stateShift;
    public float framesPerTeleport;
    public float framesPerState;
    public Sprite[] states;
    private SpriteRenderer sr;

    public GameObject projectile;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        cameraS= GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ClassicFollow>();
        Teleport();
    }

    void FixedUpdate()
    {
        //increase stateShift to move naimation along
        stateShift++;
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
        else if (Mathf.Abs(stateShift) > framesPerState * states.Length - 2)
        {
            sr.sprite = states[states.Length - 2];
        }
        else
        {
            sr.sprite = states[(int) (Mathf.Floor(Mathf.Abs(stateShift) / framesPerState))];
        }
    }

    private void Teleport()
    {
        while (IsColliding())
        {
            transform.position = new Vector3(
                cameraS.transform.position.x - cameraS.offsetX + Random.Range((int)cameraS.screenLengthX / 2 - 2, (int)-cameraS.screenLengthX / 2 + 1),
                cameraS.transform.position.y - cameraS.offsetY + Random.Range((int)cameraS.screenLengthY / 2 - 2, (int)-cameraS.screenLengthY / 2 + 1),
                -0.5f);
        }
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
