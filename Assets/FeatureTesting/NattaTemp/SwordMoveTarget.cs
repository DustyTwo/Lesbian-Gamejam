using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordMoveTarget : MonoBehaviour
{
    Camera _mainCamera;
    [SerializeField] private GameObject _player;
    [SerializeField] private float _speed;

    private Vector2 _move;

    private void Awake()
    {
        _move = (_player.transform.position - transform.position).normalized;
    }

    void FixedUpdate()
    {
        transform.Translate(_move * _speed * Time.fixedDeltaTime);
    }
}
