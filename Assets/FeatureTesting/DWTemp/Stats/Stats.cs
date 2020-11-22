using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Stat
{
    [SerializeField] private string _name;
    [SerializeField] private int _value;

    public int GetValue
    {
        get
        {
            if (_value > 255)
                return 255;
            return _value;
        }
    }

    public float GetFraction { get { return _value / 255f; } } // 255 max stat value
}
public class Stats : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
