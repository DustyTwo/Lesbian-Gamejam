using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LetterData : MonoBehaviour
{
	public TMPro.TextMeshProUGUI text;
	public HorizontalLayoutGroup layout;
	public ContentSizeFitter fitter;

	public void Init()
	{
		if (text.text == " ")
		{
			layout.enabled = false;
			fitter.enabled = false;
		}
	}
}
