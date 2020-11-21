using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowTargetSpawnerScript : MonoBehaviour
{
    [SerializeField] GameObject targetPrefab;
    [SerializeField] float targetRadius;

    [SerializeField] Vector2 targetInitialVelocityMinMaxX;
    [SerializeField] Vector2 targetInitialVelocityMinMaxY;

    Timer targetSpawnTimer;
    [SerializeField] private float baseSpawnTime;

    [SerializeField] BowComboCounter bowComboCounter;

    Camera mainCamera;

    float spawnPosXMin;
    float spawnPosXMax;
    float spawnPosY;

    private void Awake()
    {
        mainCamera = Camera.main;

        targetSpawnTimer = new Timer(baseSpawnTime);

        spawnPosXMin = mainCamera.orthographicSize * -Screen.width / Screen.height + targetRadius;
        spawnPosXMax = -spawnPosXMin;
        spawnPosY = -mainCamera.orthographicSize - targetRadius;
    }

    void Update()
    {
        targetSpawnTimer += Time.deltaTime;
        if (targetSpawnTimer.Expired)
        {
            targetSpawnTimer.Reset();
            SpawnTarget();
        }
    }

    void SpawnTarget()
    {
        Vector2 spawnPosition = new Vector2(Random.Range(spawnPosXMin, spawnPosXMax), spawnPosY);
        BowTargetScipt bowTargetScipt = Instantiate(targetPrefab, spawnPosition, Quaternion.identity).GetComponent<BowTargetScipt>();

        //Vector2 launchVector = new Vector2(Random.Range(targetInitialVelocityMinMaxX.x, targetInitialVelocityMinMaxX.y), 
        //            Random.Range(targetInitialVelocityMinMaxY.x, targetInitialVelocityMinMaxY.y));

        
        Vector2 launchVector = new Vector2(0, spawnPosition.y) - spawnPosition;

        launchVector += Vector2.ClampMagnitude(-launchVector.normalized * 3f, launchVector.magnitude);

        launchVector = Vector2.ClampMagnitude(launchVector, 2f);

        launchVector += new Vector2(Random.Range(targetInitialVelocityMinMaxX.x, targetInitialVelocityMinMaxX.y), Random.Range(targetInitialVelocityMinMaxY.x, targetInitialVelocityMinMaxY.y));

        bowTargetScipt.Initialize(launchVector, bowComboCounter);

        print(launchVector.x);

        //bowTargetScipt.Initialize(new Vector2(Random.Range(targetInitialVelocityMinMaxX.x, targetInitialVelocityMinMaxX.y), Random.Range(targetInitialVelocityMinMaxY.x, targetInitialVelocityMinMaxY.y)));
    }
}
