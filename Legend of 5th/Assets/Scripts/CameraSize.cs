using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSize : MonoBehaviour
{
    public float targetHorizontalSize = 20f;
    public float targetVerticalSize = 12f;
    // Start is called before the first frame update
    void Start()
    {
        Camera.main.orthographicSize = targetVerticalSize / 2f;

        float aspectRatio = targetHorizontalSize / targetVerticalSize;

        Camera.main.aspect = aspectRatio;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
