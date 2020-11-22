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
	public Transform playerPoint;
	public GameObject playerPrefab;
	public static MenuHandler instance;
	public ChatboxHandler dialogueHandler;
	public Transform textWindow;
	private static State gameState;

	public static CharacterHolder player;

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

		var pos = Camera.main.ScreenToWorldPoint(playerPoint.position + Vector3.right * 500f);
		player = Instantiate(playerPrefab, pos, Quaternion.identity).GetComponent<CharacterHolder>();
		DontDestroyOnLoad(player);
	}

	static bool CheckState(State state)
	{
		return gameState == state;
	}

	public static void StartConversation(DialogueBranch branch, CharacterHolder character)
	{
		if (!CheckState(State.Conversation))
		{
			gameState = State.Conversation;

			instance.dialogueHandler.currentBranch = branch;
			instance.dialogueHandler.OpenAndStartConvo();
		}
	}

	public static void ExitConversation()
	{
		gameState = State.Idle;
	}
}
