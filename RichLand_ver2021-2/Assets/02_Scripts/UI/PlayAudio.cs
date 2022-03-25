using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using UnityEngine.Audio;
using System;

public class PlayAudio : MonoBehaviour
{
    private AudioClip[] audioClips;
    public static Dictionary<int, AudioClip> audiosList = new Dictionary<int, AudioClip>();    

    private void Awake()
    {
        audioClips = Resources.LoadAll<AudioClip>("11_Audio");
        if (audioClips != null)
        {
            foreach (var clip in audioClips)
            {
                audiosList.Add(Int32.Parse(Regex.Replace(clip.name, "[^0-9]", "")), clip);
            }
        }
        audioClips = null;
    }
}
