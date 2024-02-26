using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MuteAudioButton : MonoBehaviour
{
    public Sprite unMutedAudioSprite;
    public Sprite mutedAudioSprite;

    private Button button;
    SpriteState state;

    private void Start()
    {
        button = GetComponent<Button>();

        if (GameSettings.Instance.IsAudioMutedPermanently())
        {
            state.pressedSprite = mutedAudioSprite;
            state.highlightedSprite = mutedAudioSprite;
            button.GetComponent<Image>().sprite = mutedAudioSprite;
        }
        else
        {
            state.pressedSprite = unMutedAudioSprite;
            state.highlightedSprite = unMutedAudioSprite;
            button.GetComponent<Image>().sprite = unMutedAudioSprite;
        }
    }

    private void OnGUI()
    {
        if (GameSettings.Instance.IsAudioMutedPermanently())
        {
            button.GetComponent <Image>().sprite = mutedAudioSprite;
        }
        else
        {
            button.GetComponent<Image>().sprite = unMutedAudioSprite;
        }
    }


    public void ToggleAudioIcon()
    {
        if (GameSettings.Instance.IsAudioMutedPermanently())
        {
            state.pressedSprite = unMutedAudioSprite;
            state.highlightedSprite = unMutedAudioSprite;
            GameSettings.Instance.MuteAudioPermanently(false);
        }
        else
        {
            state.pressedSprite = mutedAudioSprite;
            state.highlightedSprite = mutedAudioSprite;
            GameSettings.Instance.MuteAudioPermanently(true);
        }

        button.spriteState = state;
    }
}
