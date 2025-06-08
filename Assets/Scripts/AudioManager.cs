using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private Sound[] _sounds;
    [SerializeField] private Sound _currentPrimarySound;
    [SerializeField] private float _audioFadeMultiplier;

    [Range(0f, 1f)] public float PrimarySoundVolume;
    [Range(0f, 1f)] public float SFXVolume;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        foreach (var sound in _sounds)
        {
            sound.AudioSource = gameObject.AddComponent<AudioSource>();
            
            if (sound.AudioClips.Count == 1)
                sound.AudioSource.clip = sound.AudioClips[0];
                
            // Initialize current volume to base volume
            sound.CurrentVolume = sound.BaseVolume;
            sound.AudioSource.volume = sound.CurrentVolume;
            
            sound.AudioSource.pitch = sound.Pitch;
            sound.AudioSource.loop = sound.Loop;
        }
    }

    public void ApplyPrimarySoundVolume(float volumePercentage)
    {
        var sounds = Array.FindAll(_sounds, s => s.PrimarySound);
        foreach (var sound in sounds)
        {
            // Calculate current volume as percentage of base volume
            sound.CurrentVolume = sound.BaseVolume * volumePercentage;
            sound.AudioSource.volume = sound.CurrentVolume;
        }

        PrimarySoundVolume = volumePercentage;
    }

    public void ApplySFXVolume(float volumePercentage)
    {
        var sounds = Array.FindAll(_sounds, s => !s.PrimarySound);
        foreach (var sound in sounds)
        {
            // Calculate current volume as percentage of base volume
            sound.CurrentVolume = sound.BaseVolume * volumePercentage;
            sound.AudioSource.volume = sound.CurrentVolume;
        }
        
        SFXVolume = volumePercentage;
    }

    public void PlayEffect(SoundsList soundName)
    {
        var sound = Array.Find(_sounds, sound => sound.Name == soundName);
        
        if (sound.AudioClips != null && sound.AudioClips.Count > 0)
        {
            var currentClip = sound.AudioSource.clip;
            var availableClips = new List<AudioClip>();
        
            if (sound.AudioClips.Count > 1)
            {
                availableClips = sound.AudioClips.Where(clip => clip != currentClip).ToList();
            }
            else
            {
                availableClips = sound.AudioClips.ToList();
            }
            
            var randomIndex = Random.Range(0, availableClips.Count);
            sound.AudioSource.clip = availableClips[randomIndex];
        }
        
        sound?.AudioSource.Stop();
        sound?.AudioSource.Play();
    }

    private Coroutine _effectsCoroutine;

    public IEnumerator QueueEffects(List<SoundsList> soundQueue)
    {
        foreach (var sound in soundQueue)
        {
            PlayEffect(sound);
        
            var soundAudioSource = Array.Find(_sounds, soundAudioSource => soundAudioSource.Name == sound);
        
            yield return new WaitForSeconds(soundAudioSource.AudioSource.clip.length);
        }
    
        _effectsCoroutine = null;
    }

    public void StartEffectsQueue(List<SoundsList> soundQueue)
    {
        if (_effectsCoroutine != null)
        {
            StopCoroutine(_effectsCoroutine);
        }
    
        _effectsCoroutine = StartCoroutine(QueueEffects(soundQueue));
    }

    public void StopEffectsQueue()
    {
        if (_effectsCoroutine != null)
        {
            StopCoroutine(_effectsCoroutine);
            _effectsCoroutine = null;
        }
    }
    
    public void Stop(SoundsList soundName)
    {
        Sound sound = Array.Find(_sounds, sound => sound.Name == soundName);
        sound?.AudioSource.Stop();
    }

    public void StopAll()
    {
        foreach (var sound in _sounds)
        {
            sound.AudioSource.Stop();
        }
    }

    public void PlayPrimarySound(SoundsList primarySoundName)
    {
        if (primarySoundName == SoundsList.None)
            return;

        if (_currentPrimarySound.Name == primarySoundName)
            return;

        StartCoroutine(AudioFadeInFadeOut(primarySoundName));
    }

    private IEnumerator AudioFadeInFadeOut(SoundsList primarySoundName)
    {
        if (_currentPrimarySound?.AudioSource != null)
        {
            for (float i = _currentPrimarySound.CurrentVolume; i >= 0; i -= Time.deltaTime * _audioFadeMultiplier)
            {
                _currentPrimarySound.AudioSource.volume = i;
                yield return null;
            }

            _currentPrimarySound.AudioSource.volume = 0;
            _currentPrimarySound.AudioSource.Stop();
        }

        var sound = Array.Find(_sounds, sound => sound.Name == primarySoundName);
        if (sound == null) yield break;
    
        _currentPrimarySound = sound;
        sound.AudioSource.volume = 0;
        sound.AudioSource.Play();
        
        for (float i = 0; i <= sound.CurrentVolume; i += Time.deltaTime * _audioFadeMultiplier)
        {
            sound.AudioSource.volume = i;
            yield return null;
        }

        sound.AudioSource.volume = sound.CurrentVolume;
    }
}