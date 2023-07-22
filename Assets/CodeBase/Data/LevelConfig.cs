using System.Collections;
using System.Collections.Generic;
using CodeBase.Data;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "GameData/LevelConfig")]
public class LevelConfig : ScriptableObject
{
    [SerializeField] private int[] columnCount;

    public int GetColumnCountByLevel(int levelNum)
    {
        return levelNum < columnCount.Length - 1 ? columnCount[levelNum] : columnCount.Length - 1;
    }
}


