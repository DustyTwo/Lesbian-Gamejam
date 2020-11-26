using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum GameState
{
	Idle, Conversation, Choice	
}

public class MenuHandler : MonoBehaviour
{
	public Transform characterPoint;
	public Transform playerPoint;
	public GameObject playerPrefab;
	public static MenuHandler instance;
	public ChatboxHandler dialogueHandler;
	public Transform textWindow;
	public static GameState GameState
	{
		get;
		private set;
	}

	public static DialogueBranch CurrentBranch;
	public static int CurrentIndex;

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

	private void Update()
	{
		if(CurrentBranch != null && Input.GetMouseButtonDown(0) && dialogueHandler.active)
		{
			if(!dialogueHandler.textHandler.IsPrinting && CheckState(GameState.Conversation))
			{
				if(CurrentIndex >= CurrentBranch.dialogues.Length)
				{
					if(CurrentBranch.choices.Length > 0)
					{
						SetGameState(GameState.Choice);
						ChoiceHandler.instance.SetChoices(CurrentBranch.choices);
						ChoiceHandler.OnChoiceSelected += OnChoice;
					}
					else
					{
						dialogueHandler.CloseDialogue();
					}
				}
				else
				{
					StartCoroutine(dialogueHandler.StartDialogue());
				}
			}
		}
	}

	public void OnChoice(int index)
	{
		ChoiceHandler.OnChoiceSelected -= OnChoice;

		if (index < CurrentBranch.nextBranches.Length)
		{
			StartConversation(CurrentBranch.nextBranches[index]);
		}
		else
		{
			GameState = GameState.Idle;
			dialogueHandler.CloseDialogue();
		}
	}

	public static bool CheckState(GameState state)
	{
		return GameState == state;
	}

	public static void SetGameState(GameState gameState)
	{
		GameState = gameState;
	}

	public static void StartConversation(DialogueBranch branch)
	{
		if (!CheckState(GameState.Conversation))
		{
			GameState = GameState.Conversation;

			CurrentBranch = branch;
			CurrentIndex = 0;
			instance.dialogueHandler.OpenAndStartConvo();
		}
	}

	public static void ExitConversation()
	{
		GameState = GameState.Idle;
	}
}
