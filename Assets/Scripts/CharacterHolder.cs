using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum Character
{
	Archer, Sword, Knife, Player
}

public class CharacterHolder : MonoBehaviour
{
	public Character character;
	public DialogueBranch dialogue;
	private Vector3 startPos;
	private Vector3 startScale;
	public float largeScale = 2f;

	private void Awake()
	{
		startPos = transform.position;
		startScale = transform.localScale;
	}

	private void OnMouseDown()
	{
		MenuHandler.StartConversation(dialogue, this);
	}

	public void DoShake()
	{
		transform.DOShakeScale(0.2f, 1f, 90);
	}

	public void ReturnToNormal()
	{
		transform.DOMove(startPos, 1f);
		transform.DOScale(startScale, 1f);
	}

	public void DoBoop()
	{
		transform.DOPunchScale(Vector3.one * (.05f), .8f, 1, 0.1f).SetEase(Ease.OutBack);
	}
}
