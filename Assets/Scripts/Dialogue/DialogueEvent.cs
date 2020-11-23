using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DialogueEvent : MonoBehaviour
{
	public void StartEvent()
	{
		Instantiate(gameObject);
	}
}
