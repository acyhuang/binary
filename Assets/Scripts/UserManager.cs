using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UserManager : MonoBehaviour
{
    public List<ScoreEntry> highScores = new List<ScoreEntry>();

    public AudioSource bgm;
    public bool wantsBGM = true;
    public bool wantsSFX = true;
    
    public ColorPalette normal;
    public ColorPalette strong;
    public ColorPalette lightmode;
    public ColorPalette mocha;
    public ColorPalette lycoris;
    private ColorPalette _selectedPalette;
    public ColorPalette selectedPalette
    {
        get { return _selectedPalette; }
        set
        {
            _selectedPalette = value;
            OnPaletteChange?.Invoke(value);
        }
    }

    public static event Action<ColorPalette> OnPaletteChange;

    public static UserManager instance;
    void Awake()
    {
        selectedPalette = normal;

        LoadHighScores();
        LoadSettings();

        if (wantsBGM)
        {
            bgm.Play();
        }

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void AddHighScore(int score)
    {
        ScoreEntry newEntry = new ScoreEntry(score, DateTime.Now);
        highScores.Add(newEntry);
        SortHighScores();
        TrimHighScores();
        SaveHighScores();
    }

    private void SortHighScores()
    {
        highScores = highScores.OrderByDescending(s => s.score).ToList();
    }

    private void TrimHighScores()
    {
        if (highScores.Count > 5)
        {
            highScores = highScores.Take(5).ToList();
        }
    }

    private void SaveHighScores()
    {
        PlayerPrefs.SetInt("highScores_count", highScores.Count);
        for (int i = 0; i < highScores.Count; i++)
        {
            string scoreAsJson = JsonUtility.ToJson(highScores[i]);
            PlayerPrefs.SetString("highScores_" + i, scoreAsJson);
        }
        PlayerPrefs.Save();
    }

    private void LoadHighScores()
    {
        int count = PlayerPrefs.GetInt("highScores_count", 0);
        for (int i = 0; i < count; i++)
        {
            if (PlayerPrefs.HasKey("highScores_" + i))
            {
                string scoreAsJson = PlayerPrefs.GetString("highScores_" + i);
                ScoreEntry entry = JsonUtility.FromJson<ScoreEntry>(scoreAsJson);
                highScores.Add(entry);
            }
        }
    }

    private void SaveSettings()
    {
        PlayerPrefs.SetInt("wantsBGM", wantsBGM ? 1 : 0);
        PlayerPrefs.SetInt("wantsSFX", wantsSFX ? 1 : 0);
        PlayerPrefs.SetString("selectedPalette", selectedPalette.preset);
    }

    private void LoadSettings()
    {
        wantsBGM = PlayerPrefs.GetInt("wantsBGM", 1) == 1;
        wantsSFX = PlayerPrefs.GetInt("wantsSFX", 1) == 1;
        switch(PlayerPrefs.GetString("selectedPalette"))
        {
            case "normal":
                selectedPalette = normal;
                break;
            case "strong":
                selectedPalette = strong;
                break;
            case "lightmode":
                selectedPalette = lightmode;
                break;
            case "mocha":
                selectedPalette = mocha;
                break;
            case "lycoris":
                selectedPalette = lycoris;
                break;
            default:
                selectedPalette = normal;
                break;
        }
    }

    void OnApplicationPause(bool status)
    {
        if (status)
        {
            PlayerPrefs.Save();
        }
    }

    void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }
}
