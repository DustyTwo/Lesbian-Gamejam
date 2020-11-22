using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordShieldScript : MonoBehaviour
{
    [SerializeField] private LayerMask _blockLayerMask;
    [SerializeField] private LayerMask _attackLayerMask;

    [SerializeField] private SwordComboCounter _counter;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((_blockLayerMask & 1 << collision.gameObject.layer) == 1 << collision.gameObject.layer)
        {
            _counter.IncrementCombo();
            Destroy(collision.gameObject);
        } 
        
        if ((_attackLayerMask & 1 << collision.gameObject.layer) == 1 << collision.gameObject.layer)
        {
            Destroy(collision.gameObject);
        }
    }
}
