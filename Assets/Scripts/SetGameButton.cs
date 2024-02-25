using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SetGameButton : MonoBehaviour
{
    public enum EButtonType
    {
        NotSet,
        PairNumberButton,
        PuzzleThemeButton,
    };

    [SerializeField] public EButtonType ButtonType = EButtonType.NotSet;
    [HideInInspector] public GameSettings.EPairNumber PairNumber = GameSettings.EPairNumber.NotSet;
    [HideInInspector] public GameSettings.EPuzzleThemes PuzzleThemes = GameSettings.EPuzzleThemes.NotSet;

    private void Start()
    {
        
    }

    public void SetGameObtion(string GameSceneName)
    {
        var comp = gameObject.GetComponent<SetGameButton>();

        switch(comp.ButtonType)
        {
            case SetGameButton.EButtonType.PairNumberButton:
                GameSettings.Instance.SetPairNumber(comp.PairNumber);
                break;

            case EButtonType.PuzzleThemeButton:
                GameSettings.Instance.SetPuzzleTheme(comp.PuzzleThemes);
                break;
        }

        if(GameSettings.Instance.AllSettingsReady())
        {
            SceneManager.LoadScene(GameSceneName);
        }
    }
}
