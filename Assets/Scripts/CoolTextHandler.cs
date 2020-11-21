using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CoolTextHandler : MonoBehaviour
{
	[Multiline]
	public string testText;
    public GameObject textTemplate;
    public float printDelay = 0.02f;
    CanvasGroup canvas;
    public AudioClip printEvent;
    public new AudioSource audio;

	HorizontalLayoutGroup[] rows;

    IEnumerator Start()
    {
        canvas = GetComponent<CanvasGroup>();

        rows = GetComponentsInChildren<HorizontalLayoutGroup>();
        ResetText();

		yield return new WaitForSeconds(2f);

		Print(testText, 5f);
    }

    public void Print(string content, float time)
    {
        StartCoroutine(PrintRoutine(content, time));
    }

    void ResetText()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            for (int ii = transform.GetChild(i).childCount - 1; ii >= 0; ii--)
            {
                Destroy(transform.GetChild(i).GetChild(ii).gameObject);
            }
        }
    }

    IEnumerator PrintRoutine(string content, float time)
    {
        var rect = GetComponent<RectTransform>().rect;
        var layout = GetComponentInChildren<HorizontalLayoutGroup>();
        float xSize = (rect.size.x - layout.padding.left * 2);
        string nextWord = "";
        float xSizeCurrent = 0;
        float xIncrement = layout.spacing;
        int currentRow = 0;

        canvas.alpha = 1;

        ResetText();

        yield return new WaitForSecondsRealtime(printDelay);

        for (int i = 0; i < content.Length; i++)
        {
            var spawn = Instantiate(textTemplate, rows[currentRow].transform).GetComponentInChildren<TMPro.TextMeshProUGUI>();
            spawn.text = content[i].ToString();
            spawn.gameObject.name = spawn.text;

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
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            for (int ii = 0; ii < transform.GetChild(i).childCount; ii++)
            {
                transform.GetChild(i).GetChild(ii).GetComponent<Animator>().enabled = true;

                if (!transform.GetChild(i).GetChild(ii).name.Equals(" "))
                {
                    if (ii % 3 == 0)
                    {
						audio.PlayOneShot(printEvent);
                    }

                    yield return new WaitForSeconds(printDelay);
                }
            }
        }

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
}
