using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DaggerSliceSpawnerScript : MonoBehaviour
{
    [SerializeField] GameObject slicePrefab;
    [SerializeField] Transform sliceSpawnBasePosition;
    [SerializeField] float sliceSpawnPositionVariationMax;

    float floatTimer;
    [SerializeField] private float baseSpawnTime;
    [Range(0.9f, 1f)] [SerializeField] private float spawnTimeMultDecreasePerCombo;
    [SerializeField] private float sliceTimeToLive;
    [Range(0.9f, 1f)] [SerializeField] private float sliceTimeToLiveMultDecreasePerCombo;


    //använder BowComboCounter för det är generellt combo script men det insåg inte det när jag skrev det :)
    [SerializeField] BowComboCounter comboCounter;
    [SerializeField] DaggerHitTextScript daggerHitTextScript;


	private void OnEnable()
	{
		daggerHitTextScript.onHit += OnHit;
	}

	private void OnDisable()
	{
		daggerHitTextScript.onHit -= OnHit;
	}

	private void Update()
    {
        floatTimer += Time.deltaTime;

        //fult och dåligt men jag orkar inte 
        if (floatTimer > baseSpawnTime * Mathf.Pow(spawnTimeMultDecreasePerCombo, comboCounter.combo))
        {
            floatTimer = 0f;
            SpawnSlice();
        }
    }

    void SpawnSlice()
    {
        Quaternion spawnRot = Quaternion.Euler(0,0,Random.Range(0, 360));
        Vector3 sliceSpawnPositionVariation = new Vector3(Random.Range(-sliceSpawnPositionVariationMax, sliceSpawnPositionVariationMax), Random.Range(-sliceSpawnPositionVariationMax, sliceSpawnPositionVariationMax), 0);
        DaggerSliceScript daggerSliceScript = Instantiate(slicePrefab, sliceSpawnBasePosition.position + sliceSpawnPositionVariation, spawnRot).GetComponent<DaggerSliceScript>();

        daggerSliceScript.Initialize(sliceTimeToLive * Mathf.Pow(sliceTimeToLiveMultDecreasePerCombo, comboCounter.combo), comboCounter, daggerHitTextScript);
    }

	void OnHit()
	{
		GetComponent<Animator>().SetTrigger("Hit");
	}
}
