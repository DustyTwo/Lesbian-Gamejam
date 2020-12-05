using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ChatboxHandler : MonoBehaviour
{
	public TMPro.TextMeshProUGUI nameText;
	public DialogueBranch CurrentBranch { get => MenuHandler.CurrentBranch; }
	public CharacterHolder character;
	private Vector3 startPos;

	public float closedOffset = 600f;
	public float transitionTime = 1f;

	public bool active = false;

	int CurrentIndex { get => MenuHandler.CurrentIndex; set => MenuHandler.CurrentIndex = value; }
	private float timeSinceStart;
	private CharacterHolder[] characters;
	private CharacterHolder currentCharacter;

	public CharacterData[] characterData;
	Dictionary<Character, CharacterData> characterDictionary;

	public CoolTextHandler textHandler { get; private set; }
	Vector3 ClosedOffset { get => startPos + Vector3.right * closedOffset; }
	DialogueData CurrentDialogue { get => MenuHandler.CurrentBranch.dialogues[CurrentIndex]; }

	private void Awake()
	{
		textHandler = GetComponentInChildren<CoolTextHandler>();

		characterDictionary = new Dictionary<Character, CharacterData>();

		for(int i = 0; i < characterData.Length; i++)
		{
			characterDictionary.Add(characterData[i].character, characterData[i]);
		}
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
		var nextCharacter = CurrentDialogue.character;

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
				if (character.character == CurrentDialogue.character)
				{
					currentCharacter = character;
					characterDictionary.TryGetValue(currentCharacter.character, out var data);
					currentCharacter.SetSprite(data.sprites[(int)CurrentDialogue.emotion]);
					currentCharacter.MoveToFront();
					break;
				}
			}

			currentCharacter = characters[0];
		}

		if(CurrentDialogue.character != currentCharacter.character)
		{
			characterDictionary.TryGetValue(currentCharacter.character, out var data);
			currentCharacter.SetSprite(data.sprites[0]);
			currentCharacter.ReturnToNormal();

			for(int i = 0; i < characters.Length; i++)
			{
				if(characters[i].character == CurrentDialogue.character)
				{
					currentCharacter = characters[i];
					characterDictionary.TryGetValue(currentCharacter.character, out data);
					currentCharacter.SetSprite(data.sprites[(int)CurrentDialogue.emotion]);
					currentCharacter.MoveToFront();
				}
			}
		}
		else
		{
			characterDictionary.TryGetValue(currentCharacter.character, out var data);
			currentCharacter.SetSprite(data.sprites[(int)CurrentDialogue.emotion]);
		}
	}

	public IEnumerator StartDialogue()
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
			dialogueEvent.GetComponent<DialogueEvent>().StartEvent();
		}

		CurrentIndex++;
	}

	private void Update()
	{

	}

	public void CloseDialogue()
	{
		Close();
		if(currentCharacter)
		{
			currentCharacter.ReturnToNormal();
		}

		MenuHandler.player.ReturnToNormal();

		CurrentIndex = 0;
		textHandler.ResetText();
		MenuHandler.ExitConversation();
	}
}
