using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class SwordTargetSpawner : MonoBehaviour
{
    [SerializeField] List<GameObject> targetPrefab;
    [SerializeField] float targetRadius;

    Timer targetSpawnTimer;
    [SerializeField] private float baseRespawnTime;
    [SerializeField] private SwordComboCounter _counter;
    [Range(0.9f, 1f)] [SerializeField] private float spawnTimeMultDecreasePerCombo;

    Camera mainCamera;

    float _timer;

    float spawnPosXMin;
    float spawnPosXMax;
    float spawnPosY;

    float spawnPosYMin;
    float spawnPosYMax;
    float spawnPosX;
    float spawnPosX1;

    private void Awake()
    {
        mainCamera = Camera.main;

        spawnPosXMin = mainCamera.orthographicSize * -Screen.width / Screen.height + targetRadius;
        spawnPosXMax = -spawnPosXMin;
        spawnPosY = mainCamera.orthographicSize + targetRadius;

        //FIxa att det spawnar ifrån vänster och höger
        spawnPosYMin = mainCamera.orthographicSize;
        spawnPosYMax = -spawnPosYMin;
        spawnPosX = mainCamera.orthographicSize * 2f + targetRadius;

        spawnPosX1 = -spawnPosX;
    }

    void Start()
    {

    }


    void Update()
    {
        _timer += Time.deltaTime;

        if (_timer > baseRespawnTime * Mathf.Pow(spawnTimeMultDecreasePerCombo, _counter.combo))
        {
            _timer = 0;
            SpawnTarget();
        }
    }


    void SpawnTarget()
    {
        int spawner = Random.Range(0, 3);

        switch (spawner)
        {
            case(0):
                Instantiate(targetPrefab[Random.Range(0, 2)], new Vector3(Random.Range(spawnPosXMin, spawnPosXMax), spawnPosY) + new Vector3(0, mainCamera.transform.position.y, 0), Quaternion.identity);
                break;
            case(1):
                Instantiate(targetPrefab[Random.Range(0, 2)], new Vector3(spawnPosX, Random.Range(spawnPosYMin, spawnPosYMax)) + new Vector3(0, mainCamera.transform.position.y, 0), Quaternion.identity);
                break;
            case(2):
                Instantiate(targetPrefab[Random.Range(0, 2)], new Vector3(spawnPosX1, Random.Range(spawnPosYMin, spawnPosYMax)) + new Vector3(0, mainCamera.transform.position.y, 0), Quaternion.identity);
                break;
        }

    }
}
