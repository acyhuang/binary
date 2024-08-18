using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BitButton : MonoBehaviour
{
    public bool value = false;
    public Image buttonImage; 
    public Sprite falseSprite;
    public Sprite trueSprite; 
    private UserManager user;
    public AudioSource clickSFX;

    void Start()
    {
        user = GameObject.FindGameObjectWithTag("Player").GetComponent<UserManager>();
        buttonImage = GetComponent<Image>(); 
        buttonImage.sprite = falseSprite;
        buttonImage.color = user.selectedPalette.textPrimary;
    }

    public void ToggleState()
    {
        value = !value;
        if (value) {
            buttonImage.sprite = trueSprite;
        }
        else {
            buttonImage.sprite = falseSprite;
        }
        if (user.wantsSFX)
        { 
          clickSFX.Play();
        }
    }
}

