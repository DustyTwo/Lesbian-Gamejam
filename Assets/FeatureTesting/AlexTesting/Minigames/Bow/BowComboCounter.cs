using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BowComboCounter : MonoBehaviour
{
    [SerializeField] float baseStatIncreaseValue;
    [SerializeField] float scoreIncreasePerCombo;

    [SerializeField] TextMeshProUGUI comboText;

    int combo;

    public void IncrementCombo()
    {
        combo++;

        comboText.text = "COMBO X" + combo + " !!";

        //add score/stat baseStatIncreaseValue * scoreIncreasePerCombo * combo
        //change target spawn freq
    }

    public void ResetCombo()
    {
        combo = 0;

        comboText.text = "COMBO X" + combo + " :(";
        //reset target spawn freq
    }
}
