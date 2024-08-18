using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetBgPrimary : MonoBehaviour
{
    private Image image;
    private UserManager user;
    
    private void OnEnable()
    {
        UserManager.OnPaletteChange += ChangeColor;
    }

    private void OnDisable()
    {
        UserManager.OnPaletteChange -= ChangeColor;
    }

    void Awake()
    {
        image = GetComponent<Image>();
    }

    void Start()
    {
        user = GameObject.FindGameObjectWithTag("Player").GetComponent<UserManager>();
        ChangeColor(user.selectedPalette);
    }

    public void ChangeColor(ColorPalette palette)
    {
        image.color = palette.bgPrimary;
    }
}
