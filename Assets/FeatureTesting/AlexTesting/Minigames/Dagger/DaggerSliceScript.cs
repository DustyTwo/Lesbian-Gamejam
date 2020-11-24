using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DaggerSliceScript : MonoBehaviour
{
    Timer timer;
    BowComboCounter _comboCounter;
    DaggerHitTextScript _daggerHitTextScript;

    /// <summary>
    /// Call this after instantiation
    /// </summary>
    public void Initialize(float sliceTime, BowComboCounter comboCounter, DaggerHitTextScript daggerHitTextScript)
    {
        timer = new Timer(sliceTime);
        _comboCounter = comboCounter;
        _daggerHitTextScript = daggerHitTextScript;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer.Expired)
        {
            //Debug.Log("2 slow");

            SliceFailed();
        }
    }

    public void SliceFailed()
    {
        //DaggerHitTextScript.EnableText(_missText);

        _comboCounter.ResetCombo();
        _daggerHitTextScript.OnMiss();

        Destroy(this.gameObject);
    }
    public void SliceComplete()
    {
        //add score
        //DaggerHitTextScript.EnableText(_hitText);
        _comboCounter.IncrementCombo();
        _daggerHitTextScript.OnHit();

        Destroy(this.gameObject);
    }


}
