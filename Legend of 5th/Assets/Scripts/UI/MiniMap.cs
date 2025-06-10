using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    private RectTransform rt;
    public GameObject player;

    public float offsetX;
    public float offsetY;
    public float screenLengthX;
    public float screenLengthY;
    public float caveX;
    void Start()
    {
        rt = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (player.transform.position.x < caveX && player.transform.position.y > -30)
        {
            rt.anchoredPosition = new Vector3(
                Mathf.Round(player.transform.position.x / screenLengthX) * 70 + offsetX,
                Mathf.Round(player.transform.position.y / screenLengthY) * 42 + offsetY,
                0);
        }
    }
}
