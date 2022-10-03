using System;
using UnityEngine;

[System.Serializable]
public class SaveState
{
    [NonSerialized] private const int HAT_COUNT = 31;
    public int HighScore { set; get; }
    public int Fish { set; get; }
    public DateTime LastSaveTime { set; get; }
    public int CurrentHatIndex { set; get; }

    public byte[] UnlockedHatFlag { set; get; }

    public SaveState()
    {
        HighScore = 0;
        Fish = 0;
        LastSaveTime = DateTime.Now;
        CurrentHatIndex = 0;
        UnlockedHatFlag = new byte[HAT_COUNT];
        UnlockedHatFlag[0] = 1;
    }
}
