using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionsNavigator : MonoBehaviour
{

    [SerializeField]
    private List<GameObject> instructionsPanels = new List<GameObject>();

    private int currentIndex = 0;
    private int nextIndex = 1;


    public void IncrementPanelList()
    {
        nextIndex = currentIndex + 1;

        if(nextIndex >= instructionsPanels.Count)
        {
            nextIndex = 0;
        }

        instructionsPanels[currentIndex].SetActive(false);
        instructionsPanels[nextIndex].SetActive(true);

        currentIndex = nextIndex;
    }

    public void DecrementPanelList()
    {
        nextIndex = currentIndex - 1;

        if(nextIndex < 0)
        {
            nextIndex = instructionsPanels.Count - 1;
        }

        instructionsPanels[currentIndex].SetActive(false);
        instructionsPanels[nextIndex].SetActive(true);

        currentIndex = nextIndex;
    }
}
