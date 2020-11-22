using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FishingCameraBehaviour : MonoBehaviour
{
    private Camera _camera;
    [SerializeField] private float _minXPosition;
    [SerializeField] private Transform _follow;

    // Start is called before the first frame update
    void Start()
    {
        _camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        float xPos = _minXPosition;
        if (_follow.position.x > _minXPosition)
            xPos = _follow.position.x;

        transform.position = new Vector3(xPos, transform.position.y, _camera.transform.position.z);
    }
}
