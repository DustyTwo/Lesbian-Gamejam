using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    private const float THRESHOLD = 1f;

    private bool            _follow;
    private Timer           _timer;
    private Transform       _transform;
    private Rigidbody2D     _body;
    private SpriteRenderer _renderer;

    [SerializeField] private float _speed;
    [SerializeField] private float _minTimeInterval = 0.5f;
    [SerializeField] private float _maxTimeInterval = 2.0f;
    [SerializeField] private float _mouthOffset = 0.5f;

    [SerializeField] private float _minSize;
    [SerializeField] private float _maxSize;

    [SerializeField] private float _size;
    public float Size
    {
        get { return _size; }
        set
        {
            _size = value;
            transform.localScale = new Vector3(value / 100, value / 100, 1) * 2;
        }
    }

    private void OnValidate()
    {
        Size = _size;
    }

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        _timer = new Timer(Random.Range(_minTimeInterval, _maxTimeInterval));
        _body = GetComponent<Rigidbody2D>();
        _renderer = GetComponent<SpriteRenderer>();
        Size = Random.Range(_minSize, _maxSize);
    }

    // Update is called once per frame
    void Update()
    {
        if (!_follow)
        {
            if (_timer.Expired)
            {
                Vector2 force = new Vector2(Random.Range(-10f, 10f),
                                            Random.Range(-5f, 5f));
                force *= Random.Range(10f, 10f);

                if (_body.position.y >= FishGameManager.Instance.bounds.up - THRESHOLD && force.y > 0)
                    force.y *= -1;
                else if (_body.position.y <= FishGameManager.Instance.bounds.down + THRESHOLD && force.y < 0)
                    force.y *= -1;

                if (_body.position.x >= FishGameManager.Instance.bounds.right - THRESHOLD && force.x > 0)
                    force.x *= -1;
                else if (_body.position.x <= FishGameManager.Instance.bounds.left + THRESHOLD && force.x < 0)
                    force.x *= -1;

                _body.AddForce(force);
                RenewTimer();
            }

            _timer += Time.deltaTime;

            if (_body.velocity.x >= 0)
                _renderer.flipX = false;
            else
                _renderer.flipX = true;
        }
        else
        {
            transform.position = _transform.position + Vector3.left * _mouthOffset * transform.localScale.x;
        }
    }

    public void Follow(Transform transform)
    {
        _follow = true;
        _transform = transform;
        _renderer.flipX = false;
    }

    private void RenewTimer()
    {
        _timer = new Timer(Random.Range(_minTimeInterval, _maxTimeInterval));
    }
}