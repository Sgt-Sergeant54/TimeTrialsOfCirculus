using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.UI;

public class levelAudioManager : MonoBehaviour
{
    public static levelAudioManager Instance;

    [SerializeField] private AudioSource backgroundMusic;
    [SerializeField] private AudioSource soundEffects;

    [SerializeField] private Slider musicVolume;
    [SerializeField] private Slider effectsVolume;

    private bool setUpDone;

    private GameSettings gameSettings;

    [SerializeField] private AudioClip buttonClick;
    [SerializeField] private AudioClip powerUpClip;
    [SerializeField] private AudioClip[] countdownClips;

    private bool playBackgroundMusic;
    private bool toggleMusic;

    private void Start()
    {
        Instance = this;
        playBackgroundMusic = true;
        setUpDone = false;
    }
    private void Update()
    {
        if (!setUpDone)
        {
            string data = File.ReadAllText(Application.persistentDataPath + "/GameSettings.json");
            gameSettings = JsonUtility.FromJson<GameSettings>(data);

            backgroundMusic.volume = gameSettings.musicVolume;
            soundEffects.volume = gameSettings.soundEffectVolume;

            musicVolume.value = gameSettings.musicVolume;
            effectsVolume.value = gameSettings.soundEffectVolume;

            setUpDone = true;
        }

        if (playBackgroundMusic && toggleMusic)
        {
            backgroundMusic.Play();

            toggleMusic = false;
        }

        if (!playBackgroundMusic && toggleMusic)
        {
            backgroundMusic.Stop();

            toggleMusic = false;
        }
    }

    public void buttonClicked()
    {
        soundEffects.clip = buttonClick;
        soundEffects.Play();
    }

    public void powerUp()
    {
        soundEffects.clip = powerUpClip;
        soundEffects.Play();
    }

    public void playCountdown(int index)
    {
        soundEffects.clip = countdownClips[index];
        soundEffects.Play();
    }

    public void changeMusicVolumeSettings()
    {
        gameSettings.musicVolume = musicVolume.value;

        backgroundMusic.volume = gameSettings.musicVolume;
        

        saveData();
    }

    public void changeEffectsVolumeSettings()
    {
        gameSettings.soundEffectVolume = effectsVolume.value;
        soundEffects.volume = gameSettings.soundEffectVolume;
        saveData();
    }

    private void saveData()
    {
        string saveData = JsonUtility.ToJson(gameSettings);
        File.WriteAllText(Application.persistentDataPath + "/GameSettings.json", saveData);
    }
}
