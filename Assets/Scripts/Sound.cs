using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public SoundsList Name;
    public List<AudioClip> AudioClips;
    [Range(0f, 1f)] public float BaseVolume = 1; // Sound volume in editor
    [Range(0f, 1f)] public float CurrentVolume; // Calculated volume based on user settings and editor volume
    [Range(.1f, 3f)] public float Pitch;
    public AudioSource AudioSource;
    public bool Loop;
    public bool PrimarySound;
}

// Add new sounds to the back of the list, otherwise all AudioManager configs will go out of place
[System.Serializable]
public enum SoundsList
{
    None,
    OceanSound,
    MoneySound,
    PopSound,
    SeagullSound,
}