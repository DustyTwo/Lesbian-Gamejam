using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public enum Character
{
	Archer, Sword, Knife, Player, Narrator
}

public class CharacterHolder : MonoBehaviour
{
	public Character character;
	public DialogueBranch dialogue;
	private SpriteRenderer image;
	private BoxCollider2D col;
	private Vector3 startPos;
	private Vector3 startScale;
	public float largeScale = 2f;

	public bool hidden;
	public bool alwaysHidden = false;
	private bool inFront;

	private void Awake()
	{
		image = GetComponent<SpriteRenderer>();
		col = GetComponent<BoxCollider2D>();
		startPos = transform.position;
		startScale = transform.localScale;
	}

	private void Update()
	{
		if (!alwaysHidden)
		{
			var color = image.color;

			col.enabled = !hidden;

			if(hidden)
			{
				color.a = Mathf.Lerp(color.a, 0, Time.deltaTime * 8f);
			}
			else
			{
				color.a = Mathf.Lerp(color.a, 1, Time.deltaTime * 8f);
			}

			image.color = color;
		}

		transform.localScale = Vector3.Lerp(transform.localScale, !inFront ? startScale : startScale * largeScale, Time.deltaTime * 8f);
	}

	private void OnMouseDown()
	{
		MenuHandler.StartConversation(dialogue, this);
	}

	public void MoveToFront()
	{
		var pos = MenuHandler.instance.characterPoint.position;
		pos = Camera.main.ScreenToWorldPoint(pos);
		pos.z = 0;

		transform.DOMove(pos, 0.5f);
		inFront = true;
	}

	public void DoShake()
	{
		transform.DOShakeScale(0.2f, 1f, 90);
	}

	public void ReturnToNormal()
	{
		Debug.Log("Moving to normal");
		transform.DOMove(startPos, 1f);
		inFront = false;
	}

	public void DoBoop()
	{
		transform.DOScale(transform.localScale * 1.05f, 0.1f).SetEase(Ease.OutBack);
	}
}
