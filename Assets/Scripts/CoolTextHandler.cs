using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoolTextHandler : MonoBehaviour
{
	[TextArea(4, 10)]
	public string testText;
    public GameObject textTemplate;
    public float printDelay = 0.02f;
    public float dotDelay = 0.5f;
    CanvasGroup canvas;
    public AudioClip printEvent;
    public new AudioSource audio;

	public float spaceWidth = 20f;
	HorizontalLayoutGroup[] rows;

	public bool IsPrinting { get; private set; }

    void Start()
    {
        canvas = GetComponent<CanvasGroup>();

		rows = new HorizontalLayoutGroup[transform.childCount];

		for(int i = 0; i < transform.childCount; i++)
		{
			if(transform.GetChild(i).TryGetComponent<HorizontalLayoutGroup>(out var row))
			{
				rows[i] = row;
			}
		}

        ResetText();
    }

    public void Print(string content, float time, float delay = 0f)
    {
		IsPrinting = true;
        StartCoroutine(PrintRoutine(content, time, delay));
    }

    public void ResetText()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            for (int ii = transform.GetChild(i).childCount - 1; ii >= 0; ii--)
            {
                Destroy(transform.GetChild(i).GetChild(ii).gameObject);
            }
        }
    }

    IEnumerator PrintRoutine(string content, float time, float delay = 0f)
    {
        var rect = GetComponent<RectTransform>().rect;
        float xSize = (rect.size.x - rows[0].padding.left * 2);
        string nextWord = "";
        float xSizeCurrent = 0;
        float xIncrement = 20;
        int currentRow = 0;

        canvas.alpha = 1;

		ResetText();

        yield return new WaitForSecondsRealtime(printDelay);

        for (int i = 0; i < content.Length; i++)
        {
            var spawn = Instantiate(textTemplate, rows[currentRow].transform).GetComponentInChildren<LetterData>();
            spawn.text.text = content[i].ToString();
            spawn.gameObject.name = spawn.text.text;

            xSizeCurrent += xIncrement;

            if (content[i] == ' ')
            {
                if (content.IndexOf(" ", i + 1) != -1)
                {
                    nextWord = content.Substring(i + 1, content.IndexOf(" ", i + 1) - i);
                }
                else
                {
                    nextWord = content.Substring(i + 1, content.Length - 1 - i);
                }

                if (xSizeCurrent + nextWord.Length * xIncrement > xSize)
                {
                    currentRow++;
                    xSizeCurrent = 0;
                }
            }
			if (content[i] == '\n')
			{
				currentRow++;
				xSizeCurrent = 0;
			}

			spawn.Init(spaceWidth);
        }

		for(int i = 0; i < rows.Length; i++)
		{
			rows[i].childScaleWidth = false;			
		}

		yield return new WaitForEndOfFrame();

		for(int i = 0; i < rows.Length; i++)
		{
			rows[i].childScaleWidth = true;
		}

		for (int i = 0; i < transform.childCount; i++)
        {
			for(int ii = 0; ii < transform.GetChild(i).childCount; ii++)
			{
				transform.GetChild(i).GetChild(ii).GetComponent<Animator>().enabled = true;

				if(!transform.GetChild(i).GetChild(ii).name.Equals(" "))
				{
					if(ii % 3 == 0)
					{
						audio.PlayOneShot(printEvent);
					}

					yield return new WaitForSeconds(printDelay);
				}

				switch(transform.GetChild(i).GetChild(ii).name)
				{
					case ".":
					case "!":
					case "?":
						yield return new WaitForSeconds(dotDelay);
						break;
					case ",":
						yield return new WaitForSeconds(dotDelay * .25f);
						break;
					default:
						break;
				}
			}
		}

		if (time != 0)
		{
			yield return new WaitForSecondsRealtime(time);

			float elapsedTime = 0;

			while (elapsedTime < 1f)
			{
				canvas.alpha = Mathf.Lerp(canvas.alpha, 0, elapsedTime / 1);

				elapsedTime += Time.unscaledDeltaTime;
				yield return new WaitForEndOfFrame();
			}

			canvas.alpha = 0;
		}

		IsPrinting = false;
    }
}
