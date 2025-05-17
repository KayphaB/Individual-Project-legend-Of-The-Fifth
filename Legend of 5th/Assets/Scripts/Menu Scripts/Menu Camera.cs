using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuCamera : MonoBehaviour
{
    public float moveSpeed = 5f; // Units per second
    private bool time=false;
    public float targetY = -30f; // Y level to check against
    public string sceneToLoad = "Main Menu";
    private float startTime;
    public float delay = 15f;

    void Start()
    {
        //Get time
        startTime = Time.time;
    }

    void Update()
    {   //after 15 seconds time = true
        if (Time.time - startTime >= delay)
        {
            time = true;
        }
        //move down
        if (time)
        {
            transform.position += Vector3.down * moveSpeed * Time.deltaTime;

        }
        // if camera hits bottom reload
        float cameraBottom = Camera.main.transform.position.y - Camera.main.orthographicSize;
        if (cameraBottom <= targetY)
        {
            time=false;
            SceneManager.LoadScene(sceneToLoad);
            
        }

    }
  
}
