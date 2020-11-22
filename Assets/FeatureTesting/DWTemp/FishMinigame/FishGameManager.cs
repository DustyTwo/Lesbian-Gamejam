using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct BoundingBox
{
    public float up;
    public float down;
    public float left;
    public float right;
}

public class FishGameManager : MonoBehaviour
{
    public BoundingBox bounds;

    private static FishGameManager _instance;
    public static FishGameManager Instance { get { return _instance; } }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(new Vector3(0, bounds.down - 0.25f, 0), new Vector3(10000, 0.5f, 1)); // floor
        Gizmos.DrawCube(new Vector3(bounds.left - 0.25f, 0, 0), new Vector3(0.5f, 10000, 1)); // left limit
    }
}
