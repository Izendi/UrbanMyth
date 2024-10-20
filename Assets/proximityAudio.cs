using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class proximityAudio : MonoBehaviour
{
    // Reference to the AudioSource
    public AudioSource audioSource;

    // The target the sound will react to (e.g., player)
    private GameObject player;
    

    // The maximum distance at which the sound starts to play
    public float maxDistance = 20f;

    // Minimum volume (when the target is farthest away)
    public float minVolume = 0.1f;

    // Maximum volume (when the target is closest)
    public float maxVolume = 1f;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");

    }

    void Update()
    {
        // Calculate the distance between the target and the sound source
        float distance = Vector3.Distance(player.transform.position, transform.position);

        // If the distance is within the range, adjust the volume
        if (distance < maxDistance)
        {
            // Map the distance to a volume value (closer = louder)
            float volume = Mathf.Lerp(maxVolume, minVolume, distance / maxDistance);
            audioSource.volume = volume;
        }
        else
        {
            // If the target is too far away, set volume to minimum
            audioSource.volume = minVolume;
        }

        // Optionally ensure the sound is playing
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
}
