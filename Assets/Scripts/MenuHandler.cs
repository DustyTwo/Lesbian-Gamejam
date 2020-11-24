using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum GameState
{
	Idle, Conversation	
}

public class MenuHandler : MonoBehaviour
{

	public Transform characterPoint;
	public Transform playerPoint;
	public GameObject playerPrefab;
	public static MenuHandler instance;
	public ChatboxHandler dialogueHandler;
	public Transform textWindow;
	private static GameState gameState;

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

	public static bool CheckState(GameState state)
	{
		return gameState == state;
	}

	public static void StartConversation(DialogueBranch branch, CharacterHolder character)
	{
		if (!CheckState(GameState.Conversation))
		{
			gameState = GameState.Conversation;

			instance.dialogueHandler.currentBranch = branch;
			instance.dialogueHandler.OpenAndStartConvo();
		}
	}

	public static void ExitConversation()
	{
		gameState = GameState.Idle;
	}
}
