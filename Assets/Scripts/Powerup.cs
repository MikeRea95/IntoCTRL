using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Powerup : MonoBehaviour
{
    public PowerupType type;
    private Image[] imgs;
    private TextMeshProUGUI text;

    private void Awake() {
        imgs = GetComponentsInChildren<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetVisible(bool visible) {
        foreach (Image img in imgs) {
            img.enabled = visible;
        }
        if(text != null) {
            text.enabled = visible;
        }
    }
}

public enum PowerupType {
    Attack,
    Defense,
    Time,
    Score
}