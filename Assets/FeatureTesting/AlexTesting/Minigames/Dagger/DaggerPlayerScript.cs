using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaggerPlayerScript : MonoBehaviour
{
    [SerializeField] public LayerMask entryTargetLayerMask;
    [SerializeField] public LayerMask allTargetsLayerMask;

    [SerializeField] public float maxSliceTime;

    [HideInInspector] public Camera mainCamera;


    #region State machine saker
    public StateMachine<DaggerPlayerScript> daggerStateMachine;

    public DaggerIdleState daggerIdleState;
    public DaggerSlicingState daggerSlicingState;
    #endregion

    private void Awake()
    {
        mainCamera = Camera.main;

        #region State machine saker
        daggerStateMachine = new StateMachine<DaggerPlayerScript>(this);

        daggerIdleState = new DaggerIdleState();
        daggerSlicingState = new DaggerSlicingState();
        #endregion
    }

    private void Start()
    {
        daggerStateMachine.ChangeState(daggerIdleState);
    }

    void Update()
    {
        transform.position = GetMouseWorldPosition();
    }

    private void FixedUpdate()
    {
        daggerStateMachine.Update();
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

#region States
public class DaggerIdleState : State<DaggerPlayerScript>
{
    RaycastHit2D _hit;
    public override void EnterState(DaggerPlayerScript owner) { }

    public override void ExitState(DaggerPlayerScript owner) { }

    public override void UpdateState(DaggerPlayerScript owner)
    {
        if (Input.GetMouseButton(0))
        {
            //kolla ifall man träffar de andra och isåfall 

            _hit = Physics2D.Raycast(owner.transform.position, owner.mainCamera.transform.forward, 5f, owner.entryTargetLayerMask);
            //if (Physics2D.Raycast(owner.transform.position, owner.mainCamera.transform.forward, out _hit, 5f, owner.entryTargetLayerMask))
            if (_hit)
            {
                Debug.Log("entry hit");
                owner.daggerStateMachine.ChangeState(owner.daggerSlicingState);

            }

            //kolla om man träffar början av the path
            //isåfall, byt state och aktivera the slice path 
        }
    }
}

public class DaggerSlicingState : State<DaggerPlayerScript>
{
    RaycastHit2D _hit;
    Timer timer;
    public override void EnterState(DaggerPlayerScript owner)
    {
        Debug.Log("slice time");
        timer = new Timer(owner.maxSliceTime);
    }

    public override void ExitState(DaggerPlayerScript owner) { }

    public override void UpdateState(DaggerPlayerScript owner)
    {
        _hit = Physics2D.Raycast(owner.transform.position, owner.mainCamera.transform.forward, 5f, owner.allTargetsLayerMask);

        if (_hit)
        {
            if (_hit.collider.gameObject.CompareTag("Dagger Slice End"))
            {
                Debug.Log("GOOD SLICE !!");
                //add score
                //add combo
                owner.daggerStateMachine.ChangeState(owner.daggerIdleState);
            }
            else
            {
                timer += Time.fixedDeltaTime;
                if (timer.Expired)
                {
                    Debug.Log("2 slow");
                    //reset combo
                    owner.daggerStateMachine.ChangeState(owner.daggerIdleState);
                }
            }
        }
        else
        {
            Debug.Log("DOOOOHH I MISSED >:(");
            //reset combo
            owner.daggerStateMachine.ChangeState(owner.daggerIdleState);
        }
        


        //raycasta kontinuelingt och kolla vad man träffar
        //(generös) timer som gör att man missar ifall man är för seg?
        //träffar man ingen hitbox missar man, förlorar combo, byter state, och avaktiverar the slice path
        //träffar man gul hitbox händer inget, det är bara fine
        //träffar man blå hittbox är the slice klar, man får poäng/combo, byter state, och avaktiverar the slice path

        //träffar man röd hitbox får man mindre poäng vid slutet av the slice path (om tid finns)
    }
}
#endregion