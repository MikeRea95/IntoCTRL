using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FinalScore : MonoBehaviour
{
    TextMeshProUGUI fin;

    private void Start()
    {
        fin = GetComponent<TextMeshProUGUI>();
        fin.text = TotalScore.instance.totalScore.ToString();
    }
}
