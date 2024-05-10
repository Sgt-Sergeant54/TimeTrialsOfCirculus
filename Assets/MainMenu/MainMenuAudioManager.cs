using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuAudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource backgroundMusic;
    [SerializeField] private AudioSource otherSoundAffects;

    [SerializeField] private Slider musicVolume;
    [SerializeField] private Slider effectsVolume;

    private bool setUpDone;

    private GameSettings gameSettings;

    [SerializeField] private AudioClip buttonClick;

    private bool playBackgroundMusic;
    private bool toggleMusic;

    private string path;


    private void Start()
    {
        playBackgroundMusic = true;

        setUpDone = false;
        path = Application.persistentDataPath + "/GameSettings.json";
    }

    private void Update()
    {
        if (!setUpDone)
        {
            if (File.Exists(path))
            {
                string loadData = File.ReadAllText(path);
                gameSettings = JsonUtility.FromJson<GameSettings>(loadData);
                Debug.Log(loadData);
            }
            else
            {
                CreateGameSettings();
            }

            backgroundMusic.volume = gameSettings.musicVolume;
            otherSoundAffects.volume = gameSettings.soundEffectVolume;

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
        otherSoundAffects.clip = buttonClick;
        otherSoundAffects.Play();
    }

    private void saveData()
    {
        string saveData = JsonUtility.ToJson(gameSettings);
        Debug.Log(saveData);
        File.WriteAllText(path, saveData);
    }

    private void CreateGameSettings()
    {
        gameSettings = new GameSettings();
        gameSettings.musicVolume = 0.5f;
        gameSettings.soundEffectVolume = 0.5f;
        saveData();
    }

    public void MusicVolumeChanged()
    {
        gameSettings.musicVolume = musicVolume.value;
        
        saveData();

        backgroundMusic.volume = gameSettings.musicVolume;
        
    }

    public void effectsVolumeChanged()
    {
        gameSettings.soundEffectVolume = effectsVolume.value;
        saveData();
        otherSoundAffects.volume = gameSettings.soundEffectVolume;
    }
}
