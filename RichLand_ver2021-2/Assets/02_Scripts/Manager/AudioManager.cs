using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public void SetMasterVolume(Slider slider)
    {
        audioMixer.SetFloat("MasterVolume", slider.value);
    }
    public void SetMusicVolume(Slider slider)
    {
        audioMixer.SetFloat("MusicVolume", slider.value);
    }
    public void SetSoundEffectVolume(Slider slider)
    {
        audioMixer.SetFloat("SoundEffectVolume", slider.value);
    }
    public void btn_MasterVolumeUp(Slider slider)
    {
        slider.value += 5f;
        audioMixer.SetFloat("MasterVolume", slider.value);
    }
    public void btn_MasterVolumeDown(Slider slider)
    {
        slider.value -= 5f;
        audioMixer.SetFloat("MasterVolume", slider.value);
    }
}
