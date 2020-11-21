using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CameraManager : MonoBehaviour
{
	public static CameraManager instance;

    void Awake()
    {
		instance = this;
    }

	public static void DoShake(float time = .3f, float strength = 1f)
	{
		instance.transform.DOShakePosition(time, strength, 90);
	}
}
