using System;
using System.Collections.Generic;
using Code.Scripts.Audio;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using UnityEngine.UI.Extensions;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    
    [Header("Volume")]
    [Range(0,1)] public float masterVolume = 1;
    [Range(0,1)] public float musicVolume = 1;
    [Range(0,1)] public float sfxVolume = 1;
    [Range(0,1)] public float ambienceVolume = 1;
    
    private Bus _masterBus;
    private Bus _musicBus;
    private Bus _sfxBus;
    private Bus _ambienceBus;
    
    [SerializeField, ReadOnly] private List<string> _eventNames = new();
    [SerializeField, ReadOnly] private List<string> _emitterNames = new();
    private List<EventInstance> _eventInstances = new();
    private List<StudioEventEmitter> _emitters = new();

    [SerializeField] private SoundLibrary _soundLibrary;
    private EventInstance _ambientEventInstance;
    private EventInstance _musicEventInstance;
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else {
            Destroy(gameObject);
            return;
        }
        
        _eventInstances.Clear();
        _emitters.Clear();
        
        _masterBus = RuntimeManager.GetBus("bus:/");
        _musicBus = RuntimeManager.GetBus("bus:/Music");
        _sfxBus = RuntimeManager.GetBus("bus:/SFX");
        _ambienceBus = RuntimeManager.GetBus("bus:/Ambience");
    }

    private void Start()
    {
        InitializeAmbientSound();
        InitializeMusic();
    }
    
    private void Update()
    {
        _masterBus.setVolume(masterVolume);
        _musicBus.setVolume(musicVolume);
        _sfxBus.setVolume(sfxVolume);
        _ambienceBus.setVolume(ambienceVolume);
    }

    private void OnDestroy()
    {
        CleanUp();
    }
    
    private void InitializeAmbientSound()
    {
        _ambientEventInstance = CreateInstance(_soundLibrary.Match("Ambience").soundEvent);
        _ambientEventInstance.start();
    }
    
    public void SetAmbienceParameter(string parameter, float value)
    {
        _ambientEventInstance.setParameterByName(parameter, value);
    }

    public void SetMusicArea(MusicArea area)
    {
        _musicEventInstance.setParameterByName("area", (float)area);
    }
    
    
    private void InitializeMusic()
    {
        _musicEventInstance = CreateInstance(_soundLibrary.Match("Music").soundEvent);
        _musicEventInstance.start();
    }
    
    public void PlayOneShot(EventReference sound, Vector3 position)
    {
        RuntimeManager.PlayOneShot(sound, position);
    }

    public StudioEventEmitter InitializeEventEmitter(EventReference soundEvent, GameObject emitterObject)
    {
        StudioEventEmitter emitter = emitterObject.GetOrAddComponent<StudioEventEmitter>();
        emitter.EventReference = soundEvent;
        _emitterNames.Add(soundEvent.ToString());
        _emitters.Add(emitter);
        return emitter;
    }
    
    public EventInstance CreateInstance(EventReference soundEvent)
    {
        EventInstance eventInstance =  RuntimeManager.CreateInstance(soundEvent);
        _eventInstances.Add(eventInstance);
        _eventNames.Add(soundEvent.ToString());
        return eventInstance;
    }

    private void CleanUp()
    {
        foreach (var eventInstance in _eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }
        
        foreach (var emitter in _emitters)
        {
            emitter.Stop();
        }
        
        _eventInstances.Clear();
        _emitters.Clear();
        _emitterNames.Clear();
        _eventNames.Clear();
    }
    
    
}