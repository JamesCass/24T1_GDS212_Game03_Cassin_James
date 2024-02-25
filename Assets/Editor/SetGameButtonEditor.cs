using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SetGameButton))]
[CanEditMultipleObjects]
[System.Serializable]

public class SetGameButtonEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SetGameButton myScript = target as SetGameButton;

        switch (myScript.ButtonType)
        {
            case SetGameButton.EButtonType.PairNumberButton:
                myScript.PairNumber = (GameSettings.EPairNumber)EditorGUILayout.EnumPopup("Pair Numbers", myScript.PairNumber);
                break;

            case SetGameButton.EButtonType.PuzzleThemeButton:
                myScript.PuzzleThemes = (GameSettings.EPuzzleThemes) EditorGUILayout.EnumPopup("Puzzle Themes", myScript.PuzzleThemes);
                break;
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}
