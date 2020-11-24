using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DaggerHitTextScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI missText;
    [SerializeField] TextMeshProUGUI hitText;

    [SerializeField] float displayTime;

    TextMeshProUGUI currentText;
    Timer currentTextTimer;

    private void Awake()
    {
        currentTextTimer = new Timer(displayTime);
    }

    private void Update()
    {
        if (currentText != null)
        {
            currentTextTimer += Time.deltaTime;

            if (currentTextTimer.Expired)
            {
                currentText.enabled = false;
                currentText = null;
            }
        }
    }

    public void OnMiss()
    {
        EnableText(missText);
    }
    public void OnHit()
    {
        EnableText(hitText);
    }

    void EnableText(TextMeshProUGUI newText)
    {

        if (currentText != newText)
        {
            if (currentText != null)
                currentText.enabled = false;
            newText.enabled = true;
            currentText = newText;
        }
        currentTextTimer.Reset();
    }

    //gör fade out (ändra till coroutines?)
}
