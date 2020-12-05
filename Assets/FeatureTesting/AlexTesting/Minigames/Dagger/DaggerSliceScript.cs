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

		var sortOrder = -(int)(Time.time % 60);

		foreach(var sprite in GetComponentsInChildren<SpriteRenderer>())
		{
			sprite.sortingOrder = sortOrder;
		}

		var mask = GetComponentInChildren<SpriteMask>();

		mask.backSortingOrder = sortOrder - 1;
		mask.GetComponentInChildren<SpriteMask>().frontSortingOrder = sortOrder;

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
		StartCoroutine(DestroyRoutine());
    }
    public void SliceComplete()
    {
		_daggerHitTextScript.OnHit();

		//add score
		//DaggerHitTextScript.EnableText(_hitText);
		_comboCounter.IncrementCombo();
        _daggerHitTextScript.OnHit();

		GetComponent<Animator>().SetTrigger("Hit");

		StartCoroutine(DestroyRoutine());
    }

	public IEnumerator DestroyRoutine()
	{
		foreach(var col in GetComponentsInChildren<Collider>())
		{
			col.enabled = false;
		}

		enabled = false;

		GetComponent<Animator>().SetBool("Visible", false);

		yield return new WaitForSeconds(1.5f);

		Destroy(gameObject);
	}
}
