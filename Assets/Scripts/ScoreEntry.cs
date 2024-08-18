using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ScoreEntry
{
    public int score;
    public string date;

    public ScoreEntry(int score, DateTime date)
    {
        this.score = score;
        this.date = date.ToString("yyyy-MM-dd");
    }

    public ScoreEntry(int score, String date)
    {
        this.score = score;
        this.date = date;
    }
}
