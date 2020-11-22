using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class Fishing : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D body;
    [HideInInspector] public Vector2 initialPosition;
    public StateMachine<Fishing> stateMachine;

    [Header("References")]
    public Slider throwSlider;
    public Slider fishSlider;
    public Slider fishhookSlider;
    [SerializeField] private Text _distanceText;

    [Header("Throw")]
    public Vector2 maxInitialVelocity;
    public float throwSpeed = 1f;
    [Range(0f, 1f)] public float throwBaseMultiplier = 0.25f;
    [Range(0f, 1f)] public float throwAddativeMultiplier = 1f;

    [Header("Reeling")]
    public Vector2 reelingVelocity;
    public float reelInSpeed;
    public float reelInGravity;

    [Header("Angling")]
    public float playerBarWidth = 0.25f;
    public float playerBarSpeed = 1.0f;
    public float playerAffectedFishBarSpeed = 0.1f;
    public float fishBarSpeed = 0.4f;
    public float initialAnglingAnimationSpeed = 8; // m/s
    public float unfavorablePositionalSpeed = 0.4f;
    public float favorablePositionalSpeed = 0.4f;
    public float constantUnfavorablePositionalSpeed = 0.01f;


    // Start is called before the first frame update
    void Start()
    {
        stateMachine = new StateMachine<Fishing>(this);
        stateMachine.ChangeState(new ThrowingState());

        initialPosition = transform.position;
        body = GetComponent<Rigidbody2D>();
        body.gravityScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();
        //print(stateMachine.currentState.ToString());
        _distanceText.text = string.Format("{0:0.}", transform.position.x - FishGameManager.Instance.bounds.left) + "m";
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.GetComponent<Collider2D>().enabled = false;
        collision.GetComponent<Fish>().Follow(transform);
        //collision.GetComponent<Fish>().stateMachine.ChangeState(new FishAnglingState());
        stateMachine.ChangeState(new AnglingState());
    }
}

public class ThrowingState : State<Fishing>
{
    private bool _hasThrown;
    private bool _isCounting;
    private float _timer;
    private float _magnitude = -1;
    private float _frequency = 2;

    private float GetValueFromTime(float timer)
    {
        return (freqT(timer) - Mathf.Floor(freqT(timer))) * a(timer) + b(timer) + _magnitude / 2 + 0.5f;
    }

    private float freqT(float timer)
    {
        return _frequency * timer;
    }

    private float a(float timer)
    {
        return _magnitude * (-1 + 2 * (Mathf.Floor(freqT(timer)) % 2));
    }

    private float b(float timer)
    {
        return -_magnitude * Mathf.Floor(freqT(timer)) % 2;
    }

    public override void EnterState(Fishing owner) { }
    public override void ExitState(Fishing owner) { }

    public override void UpdateState(Fishing owner)
    {
        if (!_hasThrown)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _isCounting = true;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                owner.body.position = owner.initialPosition;
                owner.body.velocity = owner.maxInitialVelocity * (owner.throwBaseMultiplier + GetValueFromTime(_timer) * owner.throwAddativeMultiplier);

                _hasThrown = true;
                _isCounting = false;
                _timer = 0;
                owner.body.gravityScale = 1;
            }

            if (_isCounting)
            {
                _timer += Time.deltaTime * owner.throwSpeed;
                owner.throwSlider.value = GetValueFromTime(_timer);
            }
        }
        else
        {
            if (owner.transform.position.y < FishGameManager.Instance.bounds.up)
                owner.stateMachine.ChangeState(new ReelInState());
        }
    }
}
public class ReelInState : State<Fishing>
{
    public override void EnterState(Fishing owner)
    {
        owner.body.drag = 6;
        owner.body.angularDrag = 6;
        owner.body.gravityScale = owner.reelInGravity;
    }
    public override void ExitState(Fishing owner) { }
    public override void UpdateState(Fishing owner)
    {
        if (owner.transform.position.x >= FishGameManager.Instance.bounds.left)
        {
            if (Input.GetMouseButton(0))
            {
                Vector2 velocity = owner.reelingVelocity;

                velocity = velocity.normalized;

                if (owner.body.position.y > FishGameManager.Instance.bounds.up)
                    velocity.y = 0;

                owner.body.AddForce(velocity * owner.reelInSpeed);
            }
        }
        else if (owner.body.position.y <= FishGameManager.Instance.bounds.down - 0) //0 = radius
        {
            owner.stateMachine.ChangeState(new FailState());
        }
        else
        {
            owner.stateMachine.ChangeState(new SuccessState());
        }
    }
}
public class AnglingState : State<Fishing>
{
    private const float THRESHOLD = 0.2f;

    public StateMachine<AnglingState> stateMachine;

    private Timer _timer;
    private bool _lerping;
    private Vector2 _initialPosition;
    private Vector2 _goalPosition;

    private float _fishValue;
    private float _playerBarValue = 0;
    private float _positionPercentage = 1;

    private float _fishSpeed;
    public float FishSpeed { get { return _fishSpeed; } }

    public float FishBarValue
    {
        get { return _fishValue; }
        set { _fishValue = Mathf.Clamp(value, 0f, 1f); }
    }

    private bool IsPlayerWithin(Fishing owner) { return _playerBarValue < FishBarValue + owner.playerBarWidth / 2 && _playerBarValue > FishBarValue - owner.playerBarWidth / 2; }

    public override void EnterState(Fishing owner)
    {
        _lerping = true;
        _initialPosition = owner.transform.position;
        _goalPosition = new Vector2(FishGameManager.Instance.bounds.right - 5, (FishGameManager.Instance.bounds.down + FishGameManager.Instance.bounds.up) / 2);

        _fishSpeed = owner.fishBarSpeed;

        float duration = (_goalPosition.x - _initialPosition.x) / owner.initialAnglingAnimationSpeed;
        _timer = new Timer(duration);

        stateMachine = new StateMachine<AnglingState>(this);
        stateMachine.ChangeState(new FishResistState());

        owner.body.gravityScale = 0;
    }

    public override void ExitState(Fishing owner) { }

    public override void UpdateState(Fishing owner)
    {
        if (_lerping)
        {
            _timer += Time.deltaTime;

            owner.body.position = Vector2.Lerp(_initialPosition, _goalPosition, _timer.Ratio);

            if (owner.body.position.x >= _goalPosition.x - THRESHOLD)
                _lerping = false;
        }
        else
        {
            if (owner.transform.position.x >= FishGameManager.Instance.bounds.left + THRESHOLD)
            {
                stateMachine.Update();

                _playerBarValue += owner.playerBarSpeed * Time.deltaTime;

                if (Input.GetMouseButton(0))
                {
                    _playerBarValue -= owner.playerBarSpeed * Time.deltaTime * 2;

                    if (IsPlayerWithin(owner))
                    {
                        FishBarValue -= Time.deltaTime * owner.playerAffectedFishBarSpeed;
                        _positionPercentage -= Time.deltaTime * owner.favorablePositionalSpeed;
                    }
                }

                if (!IsPlayerWithin(owner))
                {
                    _positionPercentage += Time.deltaTime * owner.unfavorablePositionalSpeed;
                }
                _positionPercentage += Time.deltaTime * owner.constantUnfavorablePositionalSpeed;

                _positionPercentage = Mathf.Clamp(_positionPercentage, 0f, 1f);
                _playerBarValue     = Mathf.Clamp(_playerBarValue,     0f, 1f);

                owner.fishSlider.value = FishBarValue;
                owner.fishhookSlider.value = _playerBarValue;

                float x = Mathf.Lerp(FishGameManager.Instance.bounds.left, _goalPosition.x, _positionPercentage);
                float y = Random.Range(-0.5f, 0.5f);

                if (owner.body.position.y >= FishGameManager.Instance.bounds.up - THRESHOLD && y > 0)
                    y *= -1;
                else if (owner.body.position.y <= FishGameManager.Instance.bounds.down + THRESHOLD && y < 0)
                    y *= -1;

                owner.body.position = new Vector2(x, Time.deltaTime * y + owner.body.position.y);
            }
            else
            {
                owner.stateMachine.ChangeState(new SuccessState());
            }
        }
    }

    public class FishResistState : State<AnglingState>
    {
        // move towards right side
        private Timer _timer;
        private float _minTime = 0.3f;
        private float _maxTime = 0.7f;

        public override void EnterState(AnglingState owner)
        {
            _timer = new Timer(Random.Range(_minTime, _maxTime));
        }

        public override void ExitState(AnglingState owner) { }
        public override void UpdateState(AnglingState owner)
        {
            _timer += Time.deltaTime;

            owner.FishBarValue += Time.deltaTime * owner.FishSpeed;

            if (_timer.Expired)
            {
                if (owner.FishBarValue > 0.5f && Random.Range(0f, 1f) >= 0.7f)
                    owner.stateMachine.ChangeState(new FishPanicState());
                else
                    owner.stateMachine.ChangeState(new FishChillState());
            }
        }
    }
    public class FishChillState : State<AnglingState>
    {
        // dont really do anything
        private Timer _timer;
        private float _minTime = 0.2f;
        private float _maxTime = 2.1f;

        public override void EnterState(AnglingState owner)
        {
            if (owner._fishValue < 0.5f)
            {
                _minTime += 0.5f;
                _maxTime += 0.5f;
            }
            _timer = new Timer(Random.Range(_minTime, _maxTime));
        }
        public override void ExitState(AnglingState owner) { }
        public override void UpdateState(AnglingState owner)
        {
            _timer += Time.deltaTime;
            if (_timer.Expired)
                owner.stateMachine.ChangeState(new FishResistState());
        }
    }
    public class FishPanicState : State<AnglingState>
    {
        // Select point on slider and go there
        private Timer _timer;
        private float _minTime = 0.2f;
        private float _maxTime = 0.3f;
        private float _random;

        public override void EnterState(AnglingState owner)
        {
            _random = Random.Range(0.1f, 0.4f);
            _timer = new Timer(Random.Range(_minTime, _maxTime));
        }
        public override void ExitState(AnglingState owner) { }
        public override void UpdateState(AnglingState owner)
        {
            _timer += Time.deltaTime;

            owner.FishBarValue = Mathf.SmoothStep(owner.FishBarValue, _random, _timer.Ratio);

            if (_timer.Expired)
                owner.stateMachine.ChangeState(new FishChillState());
        }
    }
}

public class SuccessState : State<Fishing>
{

    public override void EnterState(Fishing owner)
    {
        Debug.Log("final state");
        owner.body.gravityScale = 0;
    }
    public override void ExitState(Fishing owner)
    {

    }
    public override void UpdateState(Fishing owner)
    {


    }
}

public class FailState : State<Fishing>
{

    public override void EnterState(Fishing owner)
    {
        Debug.Log("failed :(");
        owner.body.gravityScale = 0;
    }
    public override void ExitState(Fishing owner)
    {

    }
    public override void UpdateState(Fishing owner)
    {


    }
}













/*
playerValue += playerSpeed * Time.deltaTime;
fishValue -= fishSpeed * Time.deltaTime;

if (Input.GetMouseButton(0))
    playerValue -= playerSpeed * Time.deltaTime * 2;

if (!fishIncrease)
{
    fishButtonDown += Random.Range(-0.4f, 0.4f);
    Debug.Log("btn down: " + fishButtonDown);
    if (fishButtonDown >= 1)
    {
        fishIncrease = true;
        fishButtonDownTimer = new Timer(Random.Range(0.1f, 0.4f));
        fishButtonDown = 0;
    }
}
else
{
    fishValue = Mathf.SmoothStep(fishValue, Mathf.Clamp(fishButtonDownTimer.Duration / (Time.deltaTime * 2) * fishSpeed, 0f, 1f), fishButtonDownTimer.Ratio);
    fishValue += fishSpeed * Time.deltaTime * 2;
    fishButtonDownTimer += Time.deltaTime;
    if (fishButtonDownTimer.Expired)
    {
        fishIncrease = false;
        Debug.Log("expire!");
    }
}


playerValue = Mathf.Clamp(playerValue, 0f, 1f);
fishValue = Mathf.Clamp(fishValue, 0f, 1f);
fishButtonDown = Mathf.Clamp(fishButtonDown, 0f, 1f);

owner.fishSlider.value = fishValue;
owner.fishhookSlider.value = playerValue;
*/