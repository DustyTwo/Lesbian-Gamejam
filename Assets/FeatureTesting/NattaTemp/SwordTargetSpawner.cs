using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordTargetSpawner : MonoBehaviour
{
    [SerializeField] List<GameObject> targetPrefab;
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
        spawnPosY = mainCamera.orthographicSize + targetRadius;

        //FIxa att det spawnar ifrån vänster och höger
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
           Instantiate(targetPrefab[Random.Range(0, 2)], new Vector3(Random.Range(spawnPosXMin, spawnPosXMax), spawnPosY), Quaternion.identity);
    }
}
