using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menuenter : MonoBehaviour
{
    public string sceneToLoad = "Overworld"; // Replace with your actual scene name

    void Update()
    {
        //if your press enter load the game
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
