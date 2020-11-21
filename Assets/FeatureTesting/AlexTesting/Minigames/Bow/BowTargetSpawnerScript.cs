using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowTargetSpawnerScript : MonoBehaviour
{
    [SerializeField] GameObject targetPrefab;
    [SerializeField] float targetRadius;

    Timer targetSpawnTimer;
    [SerializeField] private float baseRespawnTime;

    Camera mainCamera;

    float spawnPosXMin;
    float spawnPosXMax;
    float spawnPosY;

    private void Awake()
    {
        mainCamera = Camera.main;

        targetSpawnTimer = new Timer(baseRespawnTime);

        spawnPosXMin = mainCamera.orthographicSize * -Screen.width / Screen.height + targetRadius;
        spawnPosXMax = -spawnPosXMin;
        spawnPosY = -mainCamera.orthographicSize - targetRadius;

        //spawnBounds[0] = new Vector2(mainCamera.orthographicSize * -Screen.width / Screen.height + targetRadius, -mainCamera.orthographicSize - targetRadius);
        //spawnBounds[1] = new Vector2(-spawnBounds[0].x, spawnBounds[0].y);
    }

    void Start()
    {

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
        Instantiate(targetPrefab, new Vector3(Random.Range(spawnPosXMin, spawnPosXMax), spawnPosY), Quaternion.identity);
    }
}
