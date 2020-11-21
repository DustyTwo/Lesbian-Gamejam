using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordCursorScript : MonoBehaviour
{
    Camera _mainCamera;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    void Start()
    {

    }

    void Update()
    {
        transform.position = GetMouseWorldPosition();
    }

    Vector3 GetMouseWorldPosition()
    {
        Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, _mainCamera);
        vec.z = 0;
        return vec;
    }

    Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
    {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }
}
