using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    public StateMachine<Fish> stateMachine;
    public Rigidbody2D body;
    public float minTimeInterval = 0.5f;
    public float maxTimeInterval = 2.0f;
    private bool _follow;
    private Transform _transform;
    private SpriteRenderer _renderer;
    [SerializeField] private float mouthOffset;
    // Start is called before the first frame update
    void Start()
    {
        stateMachine = new StateMachine<Fish>(this);
        stateMachine.ChangeState(new FishIdleState());
        body = GetComponent<Rigidbody2D>();
        _renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_follow)
        {
            stateMachine.Update();
            if (body.velocity.x >= 0)
                _renderer.flipX = false;
            else
                _renderer.flipX = true;
        }
        else
        {
            transform.position = _transform.position + Vector3.left * mouthOffset * transform.localScale.x;
        }
    }
    public void Follow(Transform transform)
    {
        _follow = true;
        _transform = transform;
        _renderer.flipX = false;
    }
}


public class FishIdleState : State<Fish>
{
    private Timer timer;
    private float threshold = 1f;
    public override void EnterState(Fish owner)
    {
        timer = new Timer(Random.Range(0.1f, 0.4f));
    }
    public override void ExitState(Fish owner)
    {

    }
    public override void UpdateState(Fish owner)
    {
        if (timer.Expired)
        {
            Vector2 force = new Vector2(Random.Range(-10f, 10f),
                                        Random.Range(-5f, 5f));
            force *= Random.Range(10f, 10f);

            //(owner.body.position.y <= FishGameManager.seaFloor + threshold && force.y < 0)

            if (owner.body.position.y >= FishGameManager.Instance.bounds.up - threshold && force.y > 0)
                force.y *= -1;
            else if (owner.body.position.y <= FishGameManager.Instance.bounds.down + threshold && force.y < 0)
                force.y *= -1;

            if (owner.body.position.x >= FishGameManager.Instance.bounds.right - threshold && force.x > 0)
                force.x *= -1;
            else if (owner.body.position.x <= FishGameManager.Instance.bounds.left + threshold && force.x < 0)
                force.x *= -1;

            //if (owner.body.position.x >= FishGameManager.Instance.bounds.right - threshold && force.x > 0)
            //force.x *= -1;

            owner.body.AddForce(force);
            RenewTimer(owner);
        }
        timer += Time.deltaTime;
    }


    private void RenewTimer(Fish owner)
    {
        timer = new Timer(Random.Range(owner.minTimeInterval, owner.maxTimeInterval));
    }
}