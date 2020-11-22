using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowArrowScript : MonoBehaviour
{
    float _radius;
    Vector3 _cameraForward;
    LayerMask _targetsLayerMask;

    Timer timer;

    /// <summary>
    /// Call this after instantiation
    /// </summary>
    public void Initialize(float travelTime, float radius, Vector3 cameraForward, LayerMask targetsLayerMask)
    {
        timer = new Timer(travelTime);
        _radius = radius;
        _cameraForward = cameraForward;
        _targetsLayerMask = targetsLayerMask;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer.Expired)
            TriggerCollision();
    }

    private void TriggerCollision()
    {
        //cirkle cast
        List<RaycastHit2D> _hitList = new List<RaycastHit2D>(Physics2D.CircleCastAll(transform.position, _radius, _cameraForward, 5f, _targetsLayerMask));

        if (_hitList.Count > 0)
        {
            //Debug.Log("target was hit " + _hitList.Count);

            for (int i = 0; i < _hitList.Count; i++)
            {
                _hitList[i].collider.GetComponent<BowTargetScipt>().OnHit();
            }
        }
        else
        {
            //Debug.Log("haha miss... noob");
        }

        Destroy(this.gameObject);
    }
}
