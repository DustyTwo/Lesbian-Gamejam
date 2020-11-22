using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaggerSliceScript : MonoBehaviour
{
    Timer timer;
    BowComboCounter _comboCounter;

    /// <summary>
    /// Call this after instantiation
    /// </summary>
    public void Initialize(float sliceTime, BowComboCounter comboCounter)
    {
        timer = new Timer(sliceTime);
        _comboCounter = comboCounter;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer.Expired)
        {
            Debug.Log("2 slow");

            SliceFailed();
        }
    }

    public void SliceFailed()
    {
        _comboCounter.ResetCombo();

        Destroy(this.gameObject);
    }
    public void SliceComplete()
    {
        //add score

        _comboCounter.IncrementCombo();

        Destroy(this.gameObject);
    }

}
