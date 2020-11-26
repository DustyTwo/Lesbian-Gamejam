using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChoiceContainer : MonoBehaviour {
	public TMPro.TextMeshProUGUI text;
	public Button button;
	private int index;
	public Animator animator;

	ChoiceHandler parent;

	public void SetText(string content, ChoiceHandler parent, int index)
	{
		this.parent = parent;
		text.text = content;
		button = GetComponent<Button>();
		this.index = index;
		button.onClick.AddListener(Clicked);
	}

	void Clicked()
	{
		parent.ButtonClicked(index);
	}

}
