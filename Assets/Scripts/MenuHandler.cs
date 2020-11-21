using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MenuHandler : MonoBehaviour
{
	enum State
	{
		Idle, Conversation	
	}

	public Transform characterPoint;
	public static MenuHandler instance;
	public ChatboxHandler dialogueHandler;
	public Transform textWindow;
	private static State gameState;

	void Awake()
	{
		if (instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;	
		}
	}

	static bool CheckState(State state)
	{
		return gameState == state;
	}

	public static void StartConversation(DialogueBranch branch, CharacterHolder character)
	{
		var pos = instance.characterPoint.position;
		pos = Camera.main.ScreenToWorldPoint(pos);
		pos.z = 0;

		character.transform.DOMove(pos, 0.7f);
		character.transform.DOScale(character.largeScale, 1f);

		if (!CheckState(State.Conversation))
		{
			gameState = State.Conversation;

			instance.dialogueHandler.currentBranch = branch;
			instance.dialogueHandler.character = character;
			instance.dialogueHandler.Open();
		}
	}

	public static void ExitConversation()
	{
		gameState = State.Idle;
	}
}
