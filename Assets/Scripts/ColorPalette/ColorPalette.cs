using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Color Palette", menuName = "UI/Color Palette")]
public class ColorPalette : ScriptableObject
{
    public string preset;
    public Color bgPrimary;
    public Color bgSecondary;
    public Color textPrimary;
    public Color textSecondary;
    public Color uiPrimary;
    public Color uiTeritary;
}
