using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioPlayer : MonoBehaviour
{
    public static event Action AudioFinishedAction;
    public static event Action OpeningAudioFinishedAction;
    public static event Action GeneralAudioFinishedAction;
    public static event Action ClosingAudioFinishedAction;

    [SerializeField]
    AudioSource _clickAudioSource;
    [SerializeField]
    AudioClip[] _audioClips;
    AudioSource _audioSource;
    bool _soundStarted = false;
    bool _gameFinished = false;
    string _lastAudioName;
    List<string> _externalVOsPlayed;

    private static AudioPlayer _instance;
    public static AudioPlayer Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<AudioPlayer>();
                _instance.Init();
            }
            return _instance;
        }
    }

    private void Start()
    {
        _externalVOsPlayed = new List<string>();
    }


    private void OnEnable()
    {
        ExternalPoiTrigger.EnteredZone += EnteredZone;
    }

    void EnteredZone(ExternalPoiTrigger trigger)
    {
        string name = trigger._compassPointName;
        if (!name.Equals(_lastAudioName) && !IsPlaying() && !_externalVOsPlayed.Contains(name))
        {
            _lastAudioName = name;  
            PlaySound(name);
            _externalVOsPlayed.Add(name);
        }
    }

    public void Init()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void EndGame()
    {
        _gameFinished = true;
    }

    public void PlaySound(string name)
    {
        if (!_gameFinished)
        {
            _audioSource.clip = null;
            foreach (AudioClip clip in _audioClips)
            {
                if (clip.name.ToLower().Equals(name.ToLower()))
                {
                    _audioSource.clip = clip;
                    break;
                }
            }

            if (_audioSource.clip != null)
            {
                _audioSource.Play();
                _soundStarted = true;
            }
            else
                Debug.Log("***Sound " + name + " not found!");
        }
    }


    public void PlayClick()
    {
        _clickAudioSource.Play();
    }

    public void StopSound()
    {
        _audioSource.Stop();
    }

    void Update()
    {
        CheckForSoundEnd();
    }

    public bool IsPlaying()
    {
        return _audioSource.isPlaying;
    }

    void CheckForSoundEnd()
    {
        if (_soundStarted)
        {
            if (_audioSource != null && !_audioSource.isPlaying)
            {
                _soundStarted = false;
                if (_audioSource.clip.name.Contains("Opening"))
                {
                    if (OpeningAudioFinishedAction != null)
                        OpeningAudioFinishedAction();
                }

                else if (_audioSource.clip.name.Contains("General"))
                {
                    if (GeneralAudioFinishedAction != null)
                        GeneralAudioFinishedAction();
                }
                else if (_audioSource.clip.name.Contains("Closing"))
                {
                    if (ClosingAudioFinishedAction != null)
                        ClosingAudioFinishedAction();
                }
                else
                {
                    if (AudioFinishedAction != null)
                        AudioFinishedAction();
                }
            }
        }
    }
}
