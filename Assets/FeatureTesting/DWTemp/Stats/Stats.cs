using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    [SerializeField] private string _name;
    [SerializeField] private int _value;
    private int MaxValue { get { return 255; } }
    public int Value
    {
        get
        {
            if (_value > MaxValue)
                return MaxValue;
            return _value;
        }
        set
        {
            if (value < 0)
                _value = 0;
            else if (value > MaxValue)
                _value = MaxValue;
            else
                _value = value;
        }
    }

    public Stat(string name = "")
    {
        _name = name;
        _value = 0;
    }

    public float GetFraction { get { return (float)_value / (float)MaxValue; } } // 255 max stat value
}
public class Stats : MonoBehaviour
{
    public enum StatType
    {
        Etiquette,
        Conversation,
        Dancing,
        Gardening,
        Compassion,
        Romance,
        CombatTheory,
        LongbowProficiency,
        SwordProficiency,
        DaggerProficiency,
        Loyalty,
    }

    private Dictionary<StatType, Stat> statDictionary;

    private static Stats _instance;
    public static Stats Instance { get { return _instance; } }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this);
            _instance = this;
            statDictionary = new Dictionary<StatType, Stat>();
            foreach (StatType type in (StatType[])Enum.GetValues(typeof(StatType)))
            {
                statDictionary.Add(type, new Stat(type.ToString()));
            }
        }
    }

    public void ChangeStat(StatType stat, int value)
    {
        statDictionary[stat].Value = value;
    }
}