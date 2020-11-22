using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCharactersEvent : DialogueEvent
{
	public override void StartEvent()
	{
		Instantiate(gameObject);
	}

	private void Start()
	{
		var chars = FindObjectsOfType<CharacterHolder>();

		foreach (var character in chars)
		{
			character.hidden = false;
		}
	}
}
