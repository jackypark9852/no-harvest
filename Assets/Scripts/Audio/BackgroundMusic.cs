using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    AudioSource source;
    [SerializeField] float dangerFarmValuePitch = Mathf.Pow(2, 4f/12);

    [SerializeField] float audioFadeOutTime = 5f;
    
    void Awake()
    {
        source = GetComponent<AudioSource>();
        source.Play();
    }

    void OnEnable()
    {
        FarmValue.OnDangerValueEntered += SetIncreasedPitch;
        FarmValue.OnSafetyValueEntered += SetNormalPitch;
    }

    void OnDisable()
    {
        FarmValue.OnDangerValueEntered -= SetIncreasedPitch;
        FarmValue.OnSafetyValueEntered -= SetNormalPitch;
    }

    private void SetIncreasedPitch()
    {
        source.pitch = dangerFarmValuePitch;
    }

    private void SetNormalPitch()
    {
        source.pitch = 1f;
    }

    public void StopMusic()
    {
        StartCoroutine(AudioFadeOut.FadeOut(source, audioFadeOutTime));
    }
}

public static class AudioFadeOut
{
    public static IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }
}
