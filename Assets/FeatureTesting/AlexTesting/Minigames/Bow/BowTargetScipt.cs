using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowTargetScipt : MonoBehaviour
{
    [SerializeField] float gravity = 1f;
    
    Vector2 _velocity;
    BowComboCounter _comboCounter;
    float spawnYValue;

    /// <summary>
    /// Call this after instantiation
    /// </summary>
    public void Initialize(Vector3 initialVelocity, BowComboCounter comboCounter)
    {
        _velocity = initialVelocity;
        _comboCounter = comboCounter;
    }
    private void Awake()
    {
        spawnYValue = transform.position.y - 1;
    }
    private void FixedUpdate()
    {
        _velocity.y -= gravity * Time.fixedDeltaTime;

        transform.Translate(_velocity * Time.fixedDeltaTime);

        if (transform.position.y < spawnYValue)
        {
            _comboCounter.ResetCombo();
            Destroy(this.gameObject);
        }
    }
    
    /// <summary>
    /// Call this when the target is hit
    /// </summary>
    public void OnHit()
    {
        _comboCounter.IncrementCombo();
        //byt ut mot träffat äpple
        Destroy(this.gameObject);
    }
}
