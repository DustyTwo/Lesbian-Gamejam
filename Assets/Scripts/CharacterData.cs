using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character Data", menuName = "Character Data")]
public class CharacterData : ScriptableObject 
{
	public Character character;

	[Header("Order = Neutral, Angry, Happy, UwU, Sad")]
	public Sprite[] sprites = new Sprite[5];
}
