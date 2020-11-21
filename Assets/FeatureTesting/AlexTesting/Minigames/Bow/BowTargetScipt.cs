using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowTargetScipt : MonoBehaviour
{
    [SerializeField] float gravity = 1f;
    Vector2 _velocity;

    /// <summary>
    /// Call this after instantiation
    /// </summary>
    public void Initialize(Vector3 initialVelocity)
    {
        _velocity = initialVelocity;
    }

    private void FixedUpdate()
    {
        _velocity.y -= gravity * Time.fixedDeltaTime;

        transform.Translate(_velocity * Time.fixedDeltaTime);
    }
    
    /// <summary>
    /// Call this when the target is hit
    /// </summary>
    public void OnHit()
    {
        //byt ut mot träffat äpple
        //add score eller whatever
    }
}
