using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashEvent : DialogueEvent 
{
    void Start()
    {
		var sprite = GetComponent<SpriteRenderer>();
		sprite.DOFade(1f, .3f).OnComplete(() => sprite.DOFade(0, 1f));
    }
}
