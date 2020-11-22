using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ChatboxHandler : MonoBehaviour
{
	public TMPro.TextMeshProUGUI nameText;
	public DialogueBranch currentBranch;
	public CharacterHolder character;
	private Vector3 startPos;

	CoolTextHandler textHandler;
	public float closedOffset = 600f;
	public float transitionTime = 1f;

	bool active = false;

	int currentIndex = 0;
	private float timeSinceStart;
	private CharacterHolder[] characters;
	private CharacterHolder currentCharacter;

	Vector3 ClosedOffset { get => startPos + Vector3.right * closedOffset; }
	DialogueData CurrentDialogue { get => currentBranch.dialogues[currentIndex]; }

	private void Awake()
	{
		textHandler = GetComponentInChildren<CoolTextHandler>();
	}

	void Start()
	{
		startPos = transform.position;

		transform.position = ClosedOffset;
	}

	public void OpenAndStartConvo()
	{
		characters = FindObjectsOfType<CharacterHolder>();
		SetCharacter();

		nameText.text = "";

		transform.DOMove(startPos, transitionTime).SetEase(Ease.OutBack).OnComplete(() => {
			StartCoroutine(StartDialogue());
			active = true;
		});
	}

	public void Close()
	{
		active = false;
		transform.DOMove(ClosedOffset, transitionTime * 1f).SetEase(Ease.InBack);
	}

	void SetCharacter()
	{
		var nextCharacter = currentBranch.dialogues[currentIndex].character;

		if(nextCharacter == Character.Player || nextCharacter == Character.Narrator)
		{
			if (nextCharacter == Character.Player)
			{
				MenuHandler.player.MoveToFront();
			}
			return;
		}

		if (currentCharacter == null)
		{
			foreach(var character in characters)
			{
				if (character.character == currentBranch.dialogues[currentIndex].character)
				{
					currentCharacter = character;
					currentCharacter.MoveToFront();
					break;
				}
			}

			currentCharacter = characters[0];
		}

		if(currentBranch.dialogues[currentIndex].character != currentCharacter.character)
		{
			currentCharacter.ReturnToNormal();

			for(int i = 0; i < characters.Length; i++)
			{
				if(characters[i].character == currentBranch.dialogues[currentIndex].character)
				{
					currentCharacter = characters[i];
					currentCharacter.MoveToFront();
				}
			}
		}
	}

	IEnumerator StartDialogue()
	{
		SetCharacter();

		yield return new WaitForSeconds(.25f);

		textHandler.Print(CurrentDialogue.text, 0, 1.5f);

		var nameString = CurrentDialogue.character.ToString();
		var doShake = CurrentDialogue.doShake;
		var isPlayer = CurrentDialogue.character == Character.Player;
		var isNarrator = CurrentDialogue.character == Character.Narrator;

		if(isPlayer || isNarrator)
		{
			if(doShake)
			{
				CameraManager.DoShake();
				MenuHandler.player.DoShake();
			}

			if(!isNarrator)
			{
				MenuHandler.player.DoBoop();
			}

			nameString = "You";
		}
		else
		{
			if(doShake)
			{
				currentCharacter.DoShake();
			}
			else
			{
				currentCharacter.DoBoop();
			}
		}

		if(CurrentDialogue.character == Character.Narrator)
		{
			nameString = "";
		}

		nameText.text = nameString;

		foreach (var dialogueEvent in CurrentDialogue.events)
		{
			dialogueEvent.StartEvent();
		}

		currentIndex++;
	}

	private void Update()
	{
		if(currentBranch != null && Input.GetMouseButtonDown(0) && active)
		{
			if(!textHandler.IsPrinting)
			{
				if (currentIndex >= currentBranch.dialogues.Length)
				{
					Close();
					if(currentCharacter)
					{
						currentCharacter.ReturnToNormal();
					}

					MenuHandler.player.ReturnToNormal();
					
					currentBranch = null;
					currentIndex = 0;
					textHandler.ResetText(); 
					MenuHandler.ExitConversation();
				}
				else
				{
					StartCoroutine(StartDialogue());
				}
			}
		}
	}
}
