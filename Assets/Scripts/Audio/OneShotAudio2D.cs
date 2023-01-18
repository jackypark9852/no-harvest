using UnityEngine;
using Cysharp.Threading.Tasks;

public class OneShotAudio2D : MonoBehaviour
{
    public AudioSource audioSource; 
    float clipLengthSeconds = 0f;
    bool destroyed = false;

    private void OnDestroy() {
        destroyed = true;
    }

    async void PlayClipAndDestroy()
    {
        audioSource.spatialBlend = 0f; 
        audioSource.Play();
        await UniTask.Delay(Mathf.RoundToInt(clipLengthSeconds * 1000));
        if(!destroyed) {
            Destroy(gameObject);
        }
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
    