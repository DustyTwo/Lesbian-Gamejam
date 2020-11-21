using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LetterData : MonoBehaviour
{
	public TMPro.TextMeshProUGUI text;
	public HorizontalLayoutGroup layout;
	public ContentSizeFitter fitter;
	public RectTransform rect;

	public void Init(float spaceWidth)
	{
		if (text.text == " ")
		{
			layout.enabled = false;
			fitter.enabled = false;

			rect.sizeDelta = new Vector2(spaceWidth, rect.sizeDelta.y);
		}
}
}
