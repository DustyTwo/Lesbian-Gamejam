using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowPlayerScript : MonoBehaviour
{
    [SerializeField] private float chargeTime;
    [SerializeField] private float arrowRadius;
    //ändra sen
    [SerializeField] public LayerMask targetsLayerMask;

    Timer bowChargeTimer;
    [HideInInspector] public Camera mainCamera;

    #region State machine saker
    public StateMachine<BowPlayerScript> bowStateMachine;

    public BowIdleState bowIdleState;
    public BowChargingState bowChargingState;
    public BowChargedState bowChargedState;
    #endregion


    private void Awake()
    {
        bowChargeTimer = new Timer(chargeTime);
        mainCamera = Camera.main;


        bowStateMachine = new StateMachine<BowPlayerScript>(this);

        bowIdleState = new BowIdleState();
        bowChargingState = new BowChargingState(chargeTime);
        bowChargedState = new BowChargedState(arrowRadius, mainCamera.transform.forward, targetsLayerMask);
    }

    private void Start()
    {
        bowStateMachine.ChangeState(bowIdleState);
    }


    void Update()
    {
        //ändra så spriten är ett child object så man kan sätta den nice 
        transform.position = GetMouseWorldPosition();

        bowStateMachine.Update();

    }

    #region Usefull fuctions
    Vector3 GetMouseWorldPosition()
    {
        Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, mainCamera);
        vec.z = 0;
        return vec;
    }
    Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
    {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }
    #endregion

}

#region Bow states
public class BowIdleState : State<BowPlayerScript>
{
    public override void EnterState(BowPlayerScript owner)
    {
        //Debug.Log("idle");
    }

    public override void ExitState(BowPlayerScript owner)
    { }

    public override void UpdateState(BowPlayerScript owner)
    {
        if (Input.GetMouseButtonDown(0))
        {
            owner.bowStateMachine.ChangeState(owner.bowChargingState);
        }
    }
}

public class BowChargingState : State<BowPlayerScript>
{
    Timer bowChargeTimer;
    float _chargeTime;

    public BowChargingState(float chargeTime)
    {
        _chargeTime = chargeTime;
        bowChargeTimer = new Timer(_chargeTime);
    }

    public override void EnterState(BowPlayerScript owner)
    {
        bowChargeTimer.Reset();
        owner.GetComponent<SpriteRenderer>().color = Color.yellow;
        //Debug.Log("start charge");
    }

    public override void ExitState(BowPlayerScript owner)
    { }

    public override void UpdateState(BowPlayerScript owner)
    {
        bowChargeTimer += Time.deltaTime;

        //Debug.Log("charge time " + bowChargeTimer.Time);

        if (bowChargeTimer.Expired)
        {
            owner.bowStateMachine.ChangeState(owner.bowChargedState);
        }
    }
}

public class BowChargedState : State<BowPlayerScript>
{
    float _arrowRadius;
    Vector3 _cameraForward;
    LayerMask _targetsLayerMask;
    List<RaycastHit2D> _hitList;

    public BowChargedState(float arrowRadius, Vector3 cameraForward, LayerMask targetsLayerMask)
    {
        _arrowRadius = arrowRadius;
        _cameraForward = cameraForward;
        _targetsLayerMask = targetsLayerMask;
    }

    public override void EnterState(BowPlayerScript owner)
    {
        owner.GetComponent<SpriteRenderer>().color = Color.blue;
        //Debug.Log("charged");
    }

    public override void ExitState(BowPlayerScript owner)
    { }

    public override void UpdateState(BowPlayerScript owner)
    {
        if (!Input.GetMouseButton(0))
        {
            //Debug.Log("POW!!!");

            Shoot(owner);

            owner.bowStateMachine.ChangeState(owner.bowIdleState);
        }
    }

    private void Shoot(BowPlayerScript owner)
    {

        //if (Physics2D.CircleCast(owner.transform.position, _arrowRadius, _cameraForward, owner.targetsLayerMask, _hitList, 5f))

        _hitList = new List<RaycastHit2D>(Physics2D.CircleCastAll(owner.transform.position, _arrowRadius, _cameraForward, 5f, _targetsLayerMask));

        if (_hitList.Count > 0)
        {
            //Debug.Log("target was hit " + _hitList.Count);
            owner.GetComponent<SpriteRenderer>().color = Color.green;

        }
        else
        {
            //Debug.Log("haha miss... noob");
            owner.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

}
//lägga till shooting state?
#endregion

