using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    private int settings;
    private const int SettingsNumber = 2;


    public enum EPairNumber
    {
        NotSet = 0,
        E5Pairs = 5,
        E10Pairs = 10,
        E15Pairs = 15,
        E20Pairs = 20,
    }

    public enum EPuzzleThemes
    {
        NotSet,
        Black,
        Grey,
        Blue,
    }

    public struct Settings
    {
        public EPairNumber PairsNumber;
        public EPuzzleThemes PuzzleTheme;
    }

    private Settings gameSettings;

    public static GameSettings Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(this);
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        gameSettings = new Settings();
        ResetGameSettings();


    }

    public void SetPairNumber(EPairNumber Number)
    {
        if (gameSettings.PairsNumber == EPairNumber.NotSet)
            settings++;

        gameSettings.PairsNumber = Number;  
    }

    public void SetPuzzleTheme(EPuzzleThemes cat)
    {
        if (gameSettings.PuzzleTheme == EPuzzleThemes.NotSet)
        {
            settings++;

            gameSettings.PuzzleTheme = cat;
        }
    }

    public EPairNumber GetPairNumber() { return gameSettings.PairsNumber; }

    public EPuzzleThemes GetPuzzleThemes()
    {
        return gameSettings.PuzzleTheme;
    }

    public void ResetGameSettings()
    {
        settings = 0;
        gameSettings.PuzzleTheme = EPuzzleThemes.NotSet;
        gameSettings.PairsNumber = EPairNumber.NotSet;
    }

    public bool AllSettingsReady()
    {
        return settings == SettingsNumber;
    }
}