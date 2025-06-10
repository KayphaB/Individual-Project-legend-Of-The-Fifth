using UnityEngine;

public class OverworldMusicManager : MonoBehaviour
{
    public Transform player;
    public float fadeSpeed = 1.0f;
    public float crossfadeX = 0f;
    public float yThreshold = -5f;

    public AudioSource musicA; // Left side music
    public AudioSource musicB; // Right side music
    public AudioSource priorityMusic; // Y-priority music

    private void Start()
    {
        // Determine if player starts below yThreshold
        if (player.position.y < yThreshold)
        {
            musicA.volume = 0f;
            musicB.volume = 0f;
            priorityMusic.volume = 1f;
        }
        else
        {
            float t = Mathf.InverseLerp(crossfadeX - 1f, crossfadeX + 1f, player.position.x);
            musicA.volume = 1f - t;
            musicB.volume = t;
            priorityMusic.volume = 0f;
        }

        // Start playing all tracks (they're looped, muted if needed)
        musicA.loop = true;
        musicB.loop = true;
        priorityMusic.loop = true;

        musicA.Play();
        musicB.Play();
        priorityMusic.Play();
    }

    private void Update()
    {
        UpdateVolumes();
    }

    private void UpdateVolumes()
    {
        if (player.position.y < yThreshold)
        {
            // Fade in priority music, fade out others
            priorityMusic.volume = Mathf.MoveTowards(priorityMusic.volume, 1f, fadeSpeed * Time.deltaTime);
            musicA.volume = Mathf.MoveTowards(musicA.volume, 0f, fadeSpeed * Time.deltaTime);
            musicB.volume = Mathf.MoveTowards(musicB.volume, 0f, fadeSpeed * Time.deltaTime);
        }
        else
        {
            // Resume X-based crossfade
            float t = Mathf.InverseLerp(crossfadeX - 1f, crossfadeX + 1f, player.position.x);
            musicA.volume = Mathf.MoveTowards(musicA.volume, 1f - t, fadeSpeed * Time.deltaTime);
            musicB.volume = Mathf.MoveTowards(musicB.volume, t, fadeSpeed * Time.deltaTime);
            priorityMusic.volume = Mathf.MoveTowards(priorityMusic.volume, 0f, fadeSpeed * Time.deltaTime);
        }
    }
}
