using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SwordPlayerScript : MonoBehaviour
{
    [SerializeField] private GameObject _shield;
    [SerializeField] private GameObject _shield1;
    [SerializeField] private GameObject _sword;
    [SerializeField] private float _shieldRange;
    [SerializeField] private float _swordRange;
    [SerializeField] private LayerMask _blockLayerMask;
    [SerializeField] private LayerMask _attackLayerMask;
    [SerializeField] private SwordComboCounter _counter;

    [SerializeField] private Animator anim;

    private Vector3 _shieldPosition;
    private Vector3 _swordPosition;
    private Vector2 _angle;

    [SerializeField] private GameObject _swordCollider;

    Camera _mainCamera;

    private void Awake()
    {
        _mainCamera = Camera.main;
        _shieldPosition = Vector3.zero;
    }

    void Update()
    {
        _shieldPosition = -GetMouseWorldPosition().normalized * _shieldRange;
        float rot_z = Mathf.Atan2(_shieldPosition.y, _shieldPosition.x) * Mathf.Rad2Deg;

        //flip the fucking sprite :)
        if (GetMouseWorldPosition().normalized.x > 0)
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
            _sword.transform.localRotation = Quaternion.Euler(0, 180, 0);
            _sword.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            _shield.transform.rotation = Quaternion.Euler(180f, 0f, -rot_z);
            _swordCollider.transform.rotation = Quaternion.Euler(180f, 0f, -rot_z);
        }
        else
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            _sword.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            _shield.transform.rotation = Quaternion.Euler(0f, 0f, rot_z);
            _swordCollider.transform.rotation = Quaternion.Euler(0f, 0f, rot_z);
        }

        _shield.transform.position = _shieldPosition;
        _swordCollider.transform.position = _shieldPosition;

        //hit
        if (Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("Swing");
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("SWOOSH"))
        {
            _swordCollider.SetActive(true);
            _shield1.SetActive(true);
            _shield.SetActive(false);
            
            /* eller använda sig ac detta
            if (Input.GetMouseButtonDown(1))
            {
                anim.Play("Idle");
                _shield1.SetActive(false);
                _shield.SetActive(true);
            }*/
        }
        else
        {
            _swordCollider.SetActive(false);
            _shield1.SetActive(false);
            _shield.SetActive(true);
        }
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((_blockLayerMask & 1 << collision.gameObject.layer) == 1 << collision.gameObject.layer) || ((_attackLayerMask & 1 << collision.gameObject.layer) == 1 << collision.gameObject.layer))
        {
            _counter.ResetCombo();
            Destroy(collision.gameObject);
        }
    }
}
