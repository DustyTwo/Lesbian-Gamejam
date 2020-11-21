using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowPlayerScript : MonoBehaviour
{
    [SerializeField] private float chargeTime;
    [SerializeField] public GameObject arrowPrefab;
    [SerializeField] public float arrowRadius;
    [SerializeField] public float arrowTravelTime;

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
        bowChargedState = new BowChargedState(arrowTravelTime, arrowRadius, mainCamera.transform.forward, targetsLayerMask, arrowPrefab);
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
        //(polish) lägg till en kort tid där man kan släppa utan att fastna i ett skott
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
    float _arrowTravelTime;
    float _arrowRadius;
    Vector3 _cameraForward;
    LayerMask _targetsLayerMask;
    GameObject _arrowPrefab;
    List<RaycastHit2D> _hitList;

    public BowChargedState(float arrowTravelTime, float arrowRadius, Vector3 cameraForward, LayerMask targetsLayerMask, GameObject arrowPrefab)
    {
        _arrowTravelTime = arrowTravelTime;
        _arrowRadius = arrowRadius;
        _cameraForward = cameraForward;
        _targetsLayerMask = targetsLayerMask;
        _arrowPrefab = arrowPrefab;
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

        BowArrowScript bowArrowScript = Object.Instantiate(_arrowPrefab, owner.transform.position, Quaternion.identity).GetComponent<BowArrowScript>();

        bowArrowScript.Initialize(_arrowTravelTime, _arrowRadius, _cameraForward, _targetsLayerMask);
        owner.GetComponent<SpriteRenderer>().color = Color.green;
    }

}
//lägga till shooting state?
#endregion

