using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMapHandler : MonoBehaviour
{
	private Animator animator;
	private bool open;
	public bool Open { get => open; }

	void Start()
    {
		animator = GetComponent<Animator>();
	}

	public void OpenClose()
	{
		if(!open)
		{
			animator.Play("Open");
		}
		else
		{
			animator.Play("Close");
		}

		open = !open;
	} 

	public void OnTransition()
	{
		if(open)
		{
			OpenClose();
		}
	}
}
