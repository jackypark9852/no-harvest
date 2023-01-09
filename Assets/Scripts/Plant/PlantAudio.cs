using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantAudio : MonoBehaviour
{
    public AudioSource spawnSound;
    public AudioSource destroySound;

    public void PlaySpawnSound()
    {
        Debug.LogWarningFormat("Sound played");
        spawnSound.Play();
    }

    public void PlayDestroySound()
    {
        destroySound.Play();
    }
}
