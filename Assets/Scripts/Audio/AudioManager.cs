using System.Collections;
using System.Collections.Generic;
// using UnityEditor.Networking.PlayerConnection;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource uiChoosing;
    public AudioSource uiSelected; 
    
    public void PlayUIChoosing()
    {
        uiChoosing.Play(); 
    }
    
    public void PlayUISelected()
    {
        uiSelected.Play(); 
    }
}
