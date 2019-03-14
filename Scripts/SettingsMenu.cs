using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour {

    //Налаштування
    public AudioMixer audioMixer;

	public void SetVolume (float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    public void SetFullcreen (bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        Screen.SetResolution(1366, 768, isFullscreen, 60);
    }
}
