using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;
    public float offsetX;
    public float offsetY;
    public float screenLengthX;
    public float screenLengthY;
    public float speedDecayFactor;
    private Vector3 target;
    void Start()
    {
        
    }

    void Update()
    {
        //find target (the screen the player is in and the camera should be)
        target = new Vector3(
            Mathf.Round(player.transform.position.x / screenLengthX) * screenLengthX + offsetX, 
            Mathf.Round(player.transform.position.y / screenLengthY) * screenLengthY + offsetY, 
        0);

        //find average between target screen position and current position and go to it
        transform.position = new Vector3 (
            (transform.position.x + target.x * speedDecayFactor) / (speedDecayFactor + 1), 
            (transform.position.y + target.y * speedDecayFactor) / (speedDecayFactor + 1), 
        -10);
    }
}
