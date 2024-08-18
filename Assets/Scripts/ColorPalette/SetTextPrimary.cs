using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetTextPrimary : MonoBehaviour
{
    private TextMeshProUGUI text;
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
        text = GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        user = GameObject.FindGameObjectWithTag("Player").GetComponent<UserManager>();
        ChangeColor(user.selectedPalette);
    }

    public void ChangeColor(ColorPalette palette)
    {
        text.color = palette.textPrimary;
    }
}
