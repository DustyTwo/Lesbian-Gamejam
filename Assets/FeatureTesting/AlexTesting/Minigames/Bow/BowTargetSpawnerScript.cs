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
        BowTargetScipt bowTargetScipt = Instantiate(targetPrefab, new Vector3(Random.Range(spawnPosXMin, spawnPosXMax), spawnPosY), Quaternion.identity).GetComponent<BowTargetScipt>();

        bowTargetScipt.Initialize(new Vector2(Random.Range(targetInitialVelocityMinMaxX.x, targetInitialVelocityMinMaxX.y), Random.Range(targetInitialVelocityMinMaxY.x, targetInitialVelocityMinMaxY.y)));
    }
}
