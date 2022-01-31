using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider slider;
    public float volume;

    public void SetVolume()
    {
        volume = slider.value;
        audioMixer.SetFloat("Background", Mathf.Log10(volume) * 20);
    }
    void Update()
    {
        volume = .05f;
        volume = GetComponent<AudioSource>().volume;
        GetComponent<AudioSource>().Play();
    }
}
