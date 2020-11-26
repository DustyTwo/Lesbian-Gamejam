using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceHandler : MonoBehaviour
{
	public GameObject choicePrefab;

	public static ChoiceHandler instance;

	List<ChoiceContainer> choiceList;

	public static Action<int> OnChoiceSelected;

	private void Awake()
	{
		instance = this;
		ClearChoices();
		choiceList = new List<ChoiceContainer>();
	}

	void ClearChoices()
	{
		for(int i = transform.childCount-1; i >= 0; i--)
		{
			Destroy(transform.GetChild(i).gameObject);
		}
	}

	public void SetChoices(ChoiceData[] choices)
	{
		StartCoroutine(ISetChoices(choices));
	}

	IEnumerator ISetChoices(ChoiceData[] choices)
	{
		ClearChoices();

		for(int i = 0; i < choices.Length; i++)
		{
			var choice = Instantiate(choicePrefab, transform).GetComponent<ChoiceContainer>();

			choiceList.Add(choice);

			choice.SetText(choices[i].text, this, i);
			yield return new WaitForSeconds(UnityEngine.Random.Range(0, 0.1f));
			choice.animator.SetBool("Visible", true);
		}
	}

	public void ButtonClicked(int index)
	{
		for(int i = 0; i < choiceList.Count; i++)
		{
			if (i == index)
			{
				choiceList[i].animator.SetTrigger("Select");
			}
			else
			{
				choiceList[i].animator.SetBool("Visible", false);
			}
		}

		OnChoiceSelected?.Invoke(index);
	}
}

