﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaggerPlayerScript : MonoBehaviour
{
    [SerializeField] public LayerMask entryTargetLayerMask;
    [SerializeField] public LayerMask currentSliceLayer;
	//public float daggerRadius;
	public TrailRenderer mouseTrail;
	public float mouseTrailSpeed = 8f;
	public SpriteRenderer cursor;

	[HideInInspector] public Camera mainCamera;
	public System.Action OnHit;

	#region State machine saker
	public StateMachine<DaggerPlayerScript> daggerStateMachine;

    public DaggerIdleState daggerIdleState;
    //public DaggerSlicingState daggerSlicingState;
    #endregion

    private void Awake()
    {
        mainCamera = Camera.main;
		cursor = GetComponent<SpriteRenderer>();

        #region State machine saker
        daggerStateMachine = new StateMachine<DaggerPlayerScript>(this);

        daggerIdleState = new DaggerIdleState();
        //daggerSlicingState = new DaggerSlicingState();
        #endregion
    }

    private void Start()
    {
        daggerStateMachine.ChangeState(daggerIdleState);
    }

    void Update()
    {
        transform.position = GetMouseWorldPosition();

		mouseTrail.transform.position = Vector3.Lerp(mouseTrail.transform.position, transform.position, Time.deltaTime * mouseTrailSpeed);

		Color c = Color.clear;

		if(Input.GetMouseButton(0))
		{
			c = Color.white;
		}

		transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * (1- c.a), Time.deltaTime * 32f);

		mouseTrail.startColor = Color.Lerp(mouseTrail.startColor, c, Time.deltaTime * 16f);
		mouseTrail.endColor = Color.Lerp(mouseTrail.endColor, c, Time.deltaTime * 20f);

		daggerStateMachine.Update();
    }

    //private void FixedUpdate()
    //{
    //    daggerStateMachine.Update();
    //}

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
                //Debug.Log("entry hit");
                owner.daggerStateMachine.ChangeState(new DaggerSlicingState(_hit.collider.gameObject.GetComponentInParent<DaggerSliceScript>()));
            }

            //kolla om man träffar början av the path
            //isåfall, byt state och aktivera the slice path 
        }


	}
}

public class DaggerSlicingState : State<DaggerPlayerScript>
{
    RaycastHit2D _hit;
    DaggerSliceScript _slice;
    public DaggerSlicingState(DaggerSliceScript slice)
    {
        _slice = slice;
    }

    public override void EnterState(DaggerPlayerScript owner)
    {
        BoxCollider2D[] sliceColliders = _slice.GetComponentsInChildren<BoxCollider2D>();

        foreach (BoxCollider2D item in sliceColliders)
        {
            item.gameObject.layer = LayerMask.NameToLayer("Dagger Current Slice");
        }
        //_slice.gameObject.layer = LayerMask.NameToLayer("Dagger Current Slice");
        //Debug.Break();
    }

    public override void ExitState(DaggerPlayerScript owner) { }

    public override void UpdateState(DaggerPlayerScript owner)
    {
        //TRAIL RENDERER (fast bra)

        //ändra till cirkel och fixa så det stödjer att göra flera slices samtidigt
        //_hit = Physics2D.Raycast(owner.transform.position, owner.mainCamera.transform.forward, 5f, owner.allTargetsLayerMask);
        _hit = Physics2D.Raycast(owner.transform.position, owner.mainCamera.transform.forward, 5f, owner.currentSliceLayer);

        //List<RaycastHit2D> _hitList = new List<RaycastHit2D>(Physics2D.CircleCastAll(owner.transform.position, owner.daggerRadius, owner.mainCamera.transform.forward, 5f, owner.allTargetsLayerMask));

        if (_hit)
        {
            if (_hit.collider.gameObject.CompareTag("Dagger Slice End"))
            {
                //Debug.Log("GOOD SLICE !!");
                //owner.GetComponent<SpriteRenderer>().color = Color.green;

                _slice.SliceComplete();

                owner.daggerStateMachine.ChangeState(owner.daggerIdleState);
            }
        }
        else
        {
            //fixa någon grace period så det blir mindre bs

            //Debug.Log("DOOOOHH I MISSED >:(");
            //owner.GetComponent<SpriteRenderer>().color = Color.red;
            
            //gör en lite wiggel och sen att den faller ner och fadear ut, sen förstörs

            if(_slice)
                _slice.SliceFailed();

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