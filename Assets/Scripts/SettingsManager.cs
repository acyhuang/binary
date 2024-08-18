using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public GameObject[] themeObjects;
    public Button[] themeButtons;

    public Toggle bgmCheckbox;
    public Toggle sfxCheckbox;

    public ColorPalette normal;
    public ColorPalette strong;
    public ColorPalette lightmode;
    public ColorPalette mocha;
    public ColorPalette lycoris;

    private UserManager user;

    void Start()
    {
        user = GameObject.FindGameObjectWithTag("Player").GetComponent<UserManager>();
        bgmCheckbox.isOn = user.wantsBGM;
        sfxCheckbox.isOn = user.wantsSFX;
    }
    
    // public void SelectUI(GameObject buttonGameObject)
    // {
    //     Image image = buttonGameObject.GetComponent<Image>();
    //     Color newColor = image.color;
    //     newColor.a = 1;
    //     image.color = newColor;
    // }

    public void SetPalette(string paletteName)
    {
        switch(paletteName)
        {
            case "normal":
                user.selectedPalette = normal;
                break;
            case "strong":
                user.selectedPalette = strong;
                break;
            case "lightmode":
                user.selectedPalette = lightmode;
                break;
            case "mocha":
                user.selectedPalette = mocha;
                break;
            case "lycoris":
                user.selectedPalette = lycoris;
                break;
            default:
                user.selectedPalette = normal;
                break;
        }
    }

    public void ToggleBGM()
    {
        user.wantsBGM = bgmCheckbox.isOn;
        if (user.wantsBGM)
        {
            user.bgm.Play();
        }
        else
        {
            user.bgm.Stop();
        }
    }

    public void ToggleSFX()
    {
        user.wantsSFX = sfxCheckbox.isOn;
    }
}
