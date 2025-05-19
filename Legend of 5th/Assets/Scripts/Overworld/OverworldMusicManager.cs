using UnityEngine;

public class OverworldMusicManager : MonoBehaviour
{
    public Transform player; // Assign your player transform in the Inspector
    public float fadeSpeed = 1.0f; // Speed of fading
    public float crossfadeX = 0f; // X position to crossfade

    public AudioSource musicA; // Left side
    public AudioSource musicB; // Right side

    private void Start()
    {
        // Ensure both are looping
        musicA.loop = true;
        musicB.loop = true;

        // Play both tracks
        musicA.Play();
        musicB.Play();

        // Set initial volume based on player's starting position
        float t = Mathf.InverseLerp(crossfadeX - 1f, crossfadeX + 1f, player.position.x);
        musicA.volume = 1f - t;
        musicB.volume = t;
    }

    private void Update()
    {
        float t = Mathf.InverseLerp(crossfadeX - 1f, crossfadeX + 1f, player.position.x);

        // Smooth fade
        musicA.volume = Mathf.MoveTowards(musicA.volume, 1f - t, fadeSpeed * Time.deltaTime);
        musicB.volume = Mathf.MoveTowards(musicB.volume, t, fadeSpeed * Time.deltaTime);
    }
}
