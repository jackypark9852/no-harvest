using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaturalDisasterAudio : MonoBehaviour
{
    public AudioClip clip;
    public float volume = 1f;
    public GameObject oneShotAudio2DPrefab; 
    
    public void PlayAudio()
    {
        GameObject oneShotAudioObject = Object.Instantiate(oneShotAudio2DPrefab, transform.position, Quaternion.identity);
        OneShotAudio2D oneShotAudio2D = oneShotAudioObject.GetComponent<OneShotAudio2D>();
        oneShotAudio2D.SetClip(clip);
        oneShotAudio2D.SetVolume(volume); 
        oneShotAudio2D.Play(); 
    }
}
