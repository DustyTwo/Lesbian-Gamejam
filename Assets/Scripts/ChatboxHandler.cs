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

	Vector3 ClosedOffset { get => startPos + Vector3.right * closedOffset; }

	private void Awake()
	{
		textHandler = GetComponentInChildren<CoolTextHandler>();
	}

	void Start()
	{
		startPos = transform.position;

		transform.position = ClosedOffset;
	}

	public void Open()
	{
		transform.DOMove(startPos, transitionTime).SetEase(Ease.OutBack).OnComplete(() => {
			StartDialogue();
			active = true;
		});
	}

	public void Close()
	{
		active = false;
		transform.DOMove(ClosedOffset, transitionTime * 1f).SetEase(Ease.InBack);
	}

	void StartDialogue()
	{
		textHandler.Print(currentBranch.dialogues[currentIndex].text, 0, 1.5f);

		var nameString = currentBranch.character.ToString();
		var doShake = currentBranch.dialogues[currentIndex].doShake;
		var isPlayer = currentBranch.dialogues[currentIndex].isPlayer;

		if(isPlayer)
		{
			if(doShake)
				CameraManager.DoShake();

			nameString = "You";
		}
		else
		{
			if(doShake)
			{
				character.DoShake();
			}
			else
			{
				character.DoBoop();
			}
		}

		nameText.text = nameString;

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
					character.ReturnToNormal();
				}
				else
				{
					StartDialogue();
				}
			}
		}
	}
}
