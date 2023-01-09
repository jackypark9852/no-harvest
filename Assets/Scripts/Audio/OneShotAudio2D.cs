using UnityEngine;
using Cysharp.Threading.Tasks;

public class OneShotAudio2D : MonoBehaviour
{
    public AudioSource audioSource; 
    float clipLengthSeconds = 0f;

    async void PlayClipAndDestroy()
    {
        audioSource.spatialBlend = 0f; 
        audioSource.Play();
        await UniTask.Delay(Mathf.RoundToInt(clipLengthSeconds * 1000));
        Destroy(gameObject);
        return; 
    }

    public void SetClip(AudioClip clip)
    {
        audioSource.clip = clip;
        clipLengthSeconds = audioSource.clip.length;
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }

    public void Play()
    {
        PlayClipAndDestroy();
        return;
    }
}
    